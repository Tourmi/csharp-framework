using Tourmi.EntityComponentSystem.Components;
using Tourmi.EntityComponentSystem.Components.Relations;
using Tourmi.EntityComponentSystem.Entities;
using Tourmi.EntityComponentSystem.Ids;
using Tourmi.EntityComponentSystem.Queries;
using Tourmi.EntityComponentSystem.Relations;

namespace Tourmi.EntityComponentSystem;

[TestFixture]
internal class GeneralTests
{
    private record struct StringId(string Id);
    private record struct Position(int X, int Y);
    private record struct Speed(int X, int Y);

    [Test]
    public void Test1()
    {
        var ecs = World.Create();

        var someEntity = ecs.CreateEntity();
        Assert.That(someEntity.IsAlive());

        someEntity.Kill();
        Assert.That(someEntity.IsAlive(), Is.False);

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

    [Test]
    public void TestEnsure()
    {
        var ecs = World.Create();

        var entity = ecs.CreateEntity();
        ref var name = ref entity.EnsureMutable<Name>();

        Assert.That(entity.Has<Name>());

        name = new Name("SomeName");
        Assert.That(entity.Get<Name>().Value, Is.EqualTo("SomeName"));

        var position = entity.Ensure<Position>();
        Assert.That(position, Is.Default);
    }

    [Test]
    public void TestRelation()
    {
        var ecs = World.Create();

        var parent = ecs.CreateEntity();
        var child = ecs.CreateEntity();
        var relationId = new Identifier(new RelationComponentIdentifier(parent.Id.ShortId, BuiltInRelationType.ChildOf));
        child.Add(relationId);

        Assert.That(child.Has(relationId));

        var dependency1 = ecs.CreateEntity();
        var dependency2 = ecs.CreateEntity();
        var dependent = ecs.CreateEntity();

        dependent.Add(dependency1);
        dependent.Add(dependency2);
        var relationId1 = new Identifier(new RelationComponentIdentifier(dependency1.Id.ShortId, BuiltInRelationType.DependsOn));
        var relationId2 = new Identifier(new RelationComponentIdentifier(dependency2.Id.ShortId, BuiltInRelationType.DependsOn));
        ecs.Set(dependent, relationId1, new DependsOn(DependsOn.DependencyMissingBehavior.Add, DependsOn.DependencyRemovedBehavior.RemoveThis));
        ecs.Set(dependent, relationId2, new DependsOn(DependsOn.DependencyMissingBehavior.Panic, DependsOn.DependencyRemovedBehavior.Panic));

        var relationValue1 = dependent.Get<DependsOn>(relationId1);
        var relationValue2 = dependent.Get<DependsOn>(relationId2);

        Assert.Multiple(() =>
        {
            Assert.That(relationValue1, Is.EqualTo(new DependsOn(DependsOn.DependencyMissingBehavior.Add, DependsOn.DependencyRemovedBehavior.RemoveThis)));
            Assert.That(relationValue2, Is.EqualTo(new DependsOn(DependsOn.DependencyMissingBehavior.Panic, DependsOn.DependencyRemovedBehavior.Panic)));
        });
    }

    [Test]
    public void TestSystems()
    {
        var ecs = World.Create();
        ecs.AddSystem(System1);
        var someName1 = ecs.CreateEntity();
        someName1.Set<Name>(new("SomeName1"));
        someName1.Set<StringId>(new("1"));
        someName1.Set<Position>(new(1, 1));

        var someName2 = ecs.CreateEntity();
        someName2.Set<Name>(new("SomeName2"));
        someName2.Set<StringId>(new("2"));
        someName2.Set<Position>(new(2, 2));

        var ignored = ecs.CreateEntity();
        ignored.Set<Name>(new("WrongName"));
        someName2.Set<StringId>(new("3"));
        ignored.Set<Position>(new(3, 3));

        static float System1(Identifier id, in StringId stringId, ref readonly Name name, ref Position position, Ref<Speed> speed)
        {
            if (name.Value.Contains("SomeName", StringComparison.InvariantCultureIgnoreCase))
            {
                position.X += 1;
                position.Y += 1;
                speed.Reference = new Speed(1, 1);
                return 1;
            }

            speed.Reference = default;
            return 2;
        }
    }
}
