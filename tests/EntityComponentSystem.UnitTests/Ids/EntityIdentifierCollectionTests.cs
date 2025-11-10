using System.Diagnostics.CodeAnalysis;
using Tourmi.EntityComponentSystem.Archetypes;

namespace Tourmi.EntityComponentSystem.Ids;

[TestFixture(TestOf = typeof(EntityIdentifierCollection))]
[SuppressMessage("Style", "IDE0022:Use expression body for method", Justification = "Easier to adjust and read tests")]
internal class EntityIdentifierCollectionTests
{
    private EntityIdentifierCollection? _entities;

    private EntityIdentifierCollection Entities => _entities.ThrowIfNull();

    [SetUp]
    public void SetUp()
    {
        _entities = new EntityIdentifierCollection(Archetype.Create());
    }

    [Test]
    public void IsAliveReturnsFalseForDefaultId()
    {
        Assert.That(Entities.IsAlive(default), Is.False);
    }

    [Test]
    public void IsAliveReturnsFalseForInvalidId()
    {
        Assert.That(Entities.IsAlive(new Identifier(1)), Is.False);
    }

    [Test]
    public void IsAliveReturnsFalseForInvalidIdVersion()
    {
        var id = Entities.Create();
        var version = id.Version;

        var modifiedId = new Identifier(id.ShortId | (((ulong)version + 1) << Identifier.VersionBitOffset));

        Assert.That(Entities.IsAlive(modifiedId), Is.False);
    }

    [Test]
    public void IsAliveReturnsFalseForDeadEntities()
    {
        var id = Entities.Create();

        Entities.Kill(id);

        Assert.That(Entities.IsAlive(id), Is.False);
    }

    [Test]
    public void IsAliveReturnsTrueForNewEntities()
    {
        var id1 = Entities.Create();
        var id2 = Entities.Create();
        var id3 = Entities.Create();

        Assert.Multiple(() =>
        {
            Assert.That(Entities.IsAlive(id1), Is.True);
            Assert.That(Entities.IsAlive(id2), Is.True);
            Assert.That(Entities.IsAlive(id3), Is.True);
        });
    }

    [Test]
    public void CreateReturnsValidEntityId()
    {
        var id = Entities.Create();

        Assert.That(id.ShortId, Is.Not.Default);
    }

    [Test]
    public void CreateReusesDeadIds()
    {
        var id = Entities.Create();
        Entities.Kill(id);
        var newId = Entities.Create();

        Assert.Multiple(() =>
        {
            Assert.That(newId.ShortId, Is.EqualTo(id.ShortId));
            Assert.That(newId.Version, Is.Not.EqualTo(id.Version));
        });
    }

    [Test]
    public void CreateSkipsOverReservedIds()
    {
        Entities.Reserve(new IdentifierRegion() { Offset = 2, Amount = 1 });
        _ = Entities.Create();
        var id3 = Entities.Create();

        Assert.That(id3.ShortId, Is.EqualTo(3));
    }
}
