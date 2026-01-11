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
    private record struct SomeComponent(float SomeValue);

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

        Assert.That(entity.Get<int>(), Is.EqualTo(4));
        Assert.That(entity.Get<Name>().Value, Is.EqualTo("SomeName"));

        entity.Remove<Name>();
        Assert.That(entity.Get<Name>(), Is.Default);
        Assert.That(entity.Get<int>(), Is.EqualTo(4));
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
        var relationId1 = new Identifier(new RelationComponentIdentifier(dependency1.Id.ShortId, BuiltInRelationType.Requires));
        var relationId2 = new Identifier(new RelationComponentIdentifier(dependency2.Id.ShortId, BuiltInRelationType.Requires));
        ecs.Set(dependent, relationId1, new Requires(Requires.DependencyMissingBehavior.Add, Requires.DependencyRemovedBehavior.RemoveThis));
        ecs.Set(dependent, relationId2, new Requires(Requires.DependencyMissingBehavior.Panic, Requires.DependencyRemovedBehavior.Panic));

        var relationValue1 = dependent.Get<Requires>(relationId1);
        var relationValue2 = dependent.Get<Requires>(relationId2);

        Assert.Multiple(() =>
        {
            Assert.That(relationValue1,
                Is.EqualTo(new Requires(Requires.DependencyMissingBehavior.Add, Requires.DependencyRemovedBehavior.RemoveThis)));
            Assert.That(relationValue2,
                Is.EqualTo(new Requires(Requires.DependencyMissingBehavior.Panic, Requires.DependencyRemovedBehavior.Panic)));
        });
    }

    [Test]
    public void QueryArchetypeCaching()
    {
        var ecs = World.Create();

        var entity = ecs.CreateEntity();
        ref var name = ref entity.EnsureMutable<Name>();
        name = new("SomeName");

        using var query = Query.FromQueryParam<ParamGroup<Name, Position>>(ecs);

        Assert.That(query.GetEntityIds(), Has.None.EqualTo(entity.Id));

        ref var position = ref entity.EnsureMutable<Position>();
        position = new Position(1, 2);

        Assert.That(query.GetEntityIds(), Has.One.EqualTo(entity.Id));

        entity.Remove<Name>();

        Assert.That(query.GetEntityIds(), Has.None.EqualTo(entity.Id));
    }

    [Test]
    public void QueryWithWithout()
    {
        var ecs = World.Create();

        using var query = Query.FromQueryParam<ParamGroup<With<Name>, Without<Position>>>(ecs);

        var entity1 = ecs.CreateEntity();
        var entity2 = ecs.CreateEntity();
        entity2.Set<Name>(new("SomeName2"));
        var entity3 = ecs.CreateEntity();
        entity3.Set<Name>(new("SomeName3"));
        entity3.Set<Position>(new(1, 2));

        var entityIds = query.GetEntityIds().ToArray();

        Assert.That(entityIds, Has.None.EqualTo(entity1.Id));
        Assert.That(entityIds, Has.One.EqualTo(entity2.Id));
        Assert.That(entityIds, Has.None.EqualTo(entity3.Id));
    }

    [Test]
    public void EmptyQuery()
    {
        var ecs = World.Create();

        using var query = Query.FromQueryParam<Identifier>(ecs);

        var entity1 = ecs.CreateEntity();
        var entity2 = ecs.CreateEntity();
        entity2.Set<Name>(new("SomeName2"));
        var entity3 = ecs.CreateEntity();
        entity3.Set<Name>(new("SomeName3"));
        entity3.Set<Position>(new(1, 2));

        var entityIds = query.GetEntityIds().ToArray();

        Assert.That(entityIds, Has.One.EqualTo(entity1.Id));
        Assert.That(entityIds, Has.One.EqualTo(entity2.Id));
        Assert.That(entityIds, Has.One.EqualTo(entity3.Id));
    }

    [Test]
    public void QueryForeach()
    {
        var ecs = World.Create();

        var entity1 = ecs.CreateEntity();
        entity1.Set<Name>(new("SomeName1"));
        entity1.Set<Position>(new(1, 1));
        entity1.Set<Speed>(new(10, 20));

        var entity2 = ecs.CreateEntity();
        entity2.Set<Name>(new("SomeName2"));
        entity2.Set<Position>(new(2, 2));
        entity2.Set<Speed>(new(10, 20));

        var entity3 = ecs.CreateEntity();
        entity3.Set<Name>(new("WrongName"));
        entity3.Set<Position>(new(3, 3));
        entity3.Set<Speed>(new(10, 20));

        var entity4 = ecs.CreateEntity();
        entity4.Set<Name>(new("SomeName4"));
        entity4.Set<Position>(new(4, 4));

        using var query = Query.FromQueryParam<ParamGroup<Identifier, RefReadonly<Name>, Ref<Position>, Optional<Speed>>>(ecs);

        Assert.That(entity1.Get<Position>(), Is.EqualTo(new Position(1, 1)));
        Assert.That(entity2.Get<Position>(), Is.EqualTo(new Position(2, 2)));
        Assert.That(entity3.Get<Position>(), Is.EqualTo(new Position(3, 3)));
        Assert.That(entity4.Get<Position>(), Is.EqualTo(new Position(4, 4)));

        query.ForEach((ParamGroup<Identifier, RefReadonly<Name>, Ref<Position>, Optional<Speed>> param) =>
        {
            (var id, var name, var position, var speed) = param;
            if (!name.Reference.Value.Contains("SomeName", StringComparison.InvariantCultureIgnoreCase))
            {
                position.Reference = new(-10, -20);
                return;
            }

            position.Reference.X += speed.ValueOrDefault.X;
            position.Reference.Y += speed.ValueOrDefault.Y;
            return;
        });

        Assert.That(entity1.Get<Position>(), Is.EqualTo(new Position(11, 21)));
        Assert.That(entity2.Get<Position>(), Is.EqualTo(new Position(12, 22)));
        Assert.That(entity3.Get<Position>(), Is.EqualTo(new Position(-10, -20)));
        Assert.That(entity4.Get<Position>(), Is.EqualTo(new Position(4, 4)));
    }

    [Test]
    public void QueryEntities()
    {
        var ecs = World.Create();

        using var query = Query.FromQueryParam<Entity>(ecs);

        var entity = ecs.CreateEntity();
        var component = ecs.CreateComponent();
        var prefab = ecs.CreatePrefab();

        var entityIds = query.GetEntityIds().ToArray();

        Assert.That(entityIds, Has.One.EqualTo(entity.Id));
        Assert.That(entityIds, Has.One.EqualTo(component.Id));
        Assert.That(entityIds, Has.One.EqualTo(prefab.Id));
    }

    [Test]
    public void QueryComponents()
    {
        var ecs = World.Create();

        using var query = Query.FromQueryParam<ComponentEntity>(ecs);

        var entity = ecs.CreateEntity();
        var component = ecs.CreateComponent();
        var prefab = ecs.CreatePrefab();

        var entityIds = query.GetEntityIds().ToArray();

        Assert.That(entityIds, Has.None.EqualTo(entity.Id));
        Assert.That(entityIds, Has.One.EqualTo(component.Id));
        Assert.That(entityIds, Has.None.EqualTo(prefab.Id));
    }

    [Test]
    public void QueryPrefabs()
    {
        var ecs = World.Create();

        using var query = Query.FromQueryParam<PrefabEntity>(ecs);

        var entity = ecs.CreateEntity();
        var component = ecs.CreateComponent();
        var prefab = ecs.CreatePrefab();

        var entityIds = query.GetEntityIds().ToArray();

        Assert.That(entityIds, Has.None.EqualTo(entity.Id));
        Assert.That(entityIds, Has.None.EqualTo(component.Id));
        Assert.That(entityIds, Has.One.EqualTo(prefab.Id));
    }

    [Test]
    public void TestSystems()
    {
        var ecs = World.Create();
        ecs.AddSystem(System1);
        ecs.AddSystem(System2);
        ecs.AddSystem(System3);
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

        static SomeComponent System1(Identifier id, in StringId stringId, ref readonly Name name, ref Position position, Ref<Speed> speed)
        {
            if (name.Value.Contains("SomeName", StringComparison.InvariantCultureIgnoreCase))
            {
                position.X += 1;
                position.Y += 1;
                speed.Reference = new Speed(1, 1);
                return new(1);
            }

            speed.Reference = default;
            return new(2);
        }

        static void System2(Identifier id, Name name, StringId stringId, Ref<Speed> speed, SomeComponent someValue, Position position)
        {
            return;
        }

        static void System3(Identifier id, Name name, StringId stringId, SomeComponent someValue, Position position)
        {
            return;
        }
    }
}
