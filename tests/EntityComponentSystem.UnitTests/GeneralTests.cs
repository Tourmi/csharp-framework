using Tourmi.EntityComponentSystem.Components;
using Tourmi.EntityComponentSystem.Entities;

namespace Tourmi.EntityComponentSystem;

[TestFixture]
internal class GeneralTests
{
    private record struct Position(int X, int Y);

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

    [Test]
    public void TestRefs()
    {
        var ecs = World.Create();

        var entity = ecs.CreateEntity();
        entity.Set<Name>(new("SomeName"));
        ref var nameRef = ref entity.GetMutable<Name>();
        nameRef = new Name("NewName");

        Assert.That(entity.Get<Name>().Value, Is.EqualTo("NewName"));

        entity.Set<Position>(new(1, 2));
        ref var position = ref entity.GetMutable<Position>();
        position.X = 5;
        position.Y = 6;

        Assert.That(entity.Get<Position>(), Is.EqualTo(new Position(5, 6)));
    }
}
