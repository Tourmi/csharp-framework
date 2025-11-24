using Tourmi.EntityComponentSystem.Components;
using Tourmi.EntityComponentSystem.Entities;

namespace Tourmi.EntityComponentSystem;

[TestFixture]
internal class GeneralTests
{
    [Test]
    public void Test1()
    {
        var ecs = World.Create();

        var someEntity = ecs.CreateEntity();
        Assert.That(ecs.IsAlive(someEntity), Is.True);

        ecs.Kill(someEntity);
        Assert.That(ecs.IsAlive(someEntity), Is.False);

        var newEntity = ecs.CreateEntity();
        Assert.That(newEntity.Id.ShortId, Is.EqualTo(someEntity.Id.ShortId));
        Assert.That(newEntity.Id.Version, Is.EqualTo(1));

        var secondEntity = ecs.CreateEntity();
        Assert.That(secondEntity.Id.ShortId, Is.EqualTo(newEntity.Id.ShortId + 1));
    }

    [Test]
    public void Test2()
    {
        var ecs = World.Create();

        var entity = ecs.CreateEntity();
        entity.Set(new Name() { Value = "SomeName" });
        Assert.That(entity.Get<Name>().Value, Is.EqualTo("SomeName"));

        entity.Set(4);
        Assert.Multiple(() =>
        {
            Assert.That(entity.Get<int>(), Is.EqualTo(4));
            Assert.That(entity.Get<Name>().Value, Is.EqualTo("SomeName"));
        });

        entity.Remove<Name>();
        Assert.Multiple(() =>
        {
            Assert.That(entity.Get<Name>(), Is.Default);
            Assert.That(entity.Get<int>(), Is.EqualTo(4));
        });
    }
}
