using System.Diagnostics.CodeAnalysis;
using Tourmi.EntityComponentSystem.Archetypes;

namespace Tourmi.EntityComponentSystem.Ids;

[TestFixture(TestOf = typeof(IdentifierCollection))]
[SuppressMessage("Style", "IDE0022:Use expression body for method", Justification = "Easier to adjust and read tests")]
internal class EntityIdentifierCollectionTests
{
    private IdentifierCollection? _ids;

    private IdentifierCollection Ids => _ids.ThrowIfNull();

    [SetUp]
    public void SetUp()
    {
        _ids = new IdentifierCollection();
    }

    [Test]
    public void IsAliveReturnsFalseForDefaultId()
    {
        Assert.That(Ids.IsUsed(default), Is.False);
    }

    [Test]
    public void IsAliveReturnsFalseForInvalidId()
    {
        Assert.That(Ids.IsUsed(new Identifier(1)), Is.False);
    }

    [Test]
    public void IsAliveReturnsFalseForInvalidIdVersion()
    {
        var id = Ids.Create();
        var version = id.Version;

        var modifiedId = new Identifier(id.ShortId | (((ulong)version + 1) << Identifier.VersionBitOffset));

        Assert.That(Ids.IsUsed(modifiedId), Is.False);
    }

    [Test]
    public void IsAliveReturnsFalseForDeadEntities()
    {
        var id = Ids.Create();

        Ids.Free(id);

        Assert.That(Ids.IsUsed(id), Is.False);
    }

    [Test]
    public void IsAliveReturnsTrueForNewEntities()
    {
        var id1 = Ids.Create();
        var id2 = Ids.Create();
        var id3 = Ids.Create();

        Assert.Multiple(() =>
        {
            Assert.That(Ids.IsUsed(id1), Is.True);
            Assert.That(Ids.IsUsed(id2), Is.True);
            Assert.That(Ids.IsUsed(id3), Is.True);
        });
    }

    [Test]
    public void CreateReturnsValidEntityId()
    {
        var id = Ids.Create();

        Assert.That(id.ShortId, Is.Not.Default);
    }

    [Test]
    public void CreateReusesDeadIds()
    {
        var id = Ids.Create();
        Ids.Free(id);
        var newId = Ids.Create();

        Assert.Multiple(() =>
        {
            Assert.That(newId.ShortId, Is.EqualTo(id.ShortId));
            Assert.That(newId.Version, Is.Not.EqualTo(id.Version));
        });
    }

    [Test]
    public void CreateSkipsOverReservedIds()
    {
        Ids.Reserve(new IdentifierRegion() { Offset = 2, Amount = 1 });
        _ = Ids.Create();
        var id3 = Ids.Create();

        Assert.That(id3.ShortId, Is.EqualTo(3));
    }
}
