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
    private record struct Position(int X, int Y);
    private record struct Speed(int X, int Y);
    private record struct SomeComponent(float SomeValue);

    [Test]
    public void CreateKillAndIdRecycling()
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
    public void GetSetAndAddComponent()
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
    public void TestRelation()
    {
        var ecs = World.Create();

        var parent = ecs.CreateEntity();
        var child = ecs.CreateEntity();
        var relationId = new RelationComponentIdentifier(parent.Id.ShortId, BuiltInRelationType.ChildOf);
        child.Add(relationId);

        Assert.That(child.Has(relationId));

        var dependency1 = ecs.CreateEntity();
        var dependency2 = ecs.CreateEntity();
        var dependent = ecs.CreateEntity();

        dependent.Add(dependency1);
        dependent.Add(dependency2);
        var relationId1 = new RelationComponentIdentifier(dependency1.Id.ShortId, BuiltInRelationType.Requires);
        var relationId2 = new RelationComponentIdentifier(dependency2.Id.ShortId, BuiltInRelationType.Requires);
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
    public void TestStronglyTypedRelations()
    {
        var ecs = World.Create();

        var entity = ecs.CreateEntity();
        entity.Add<Relation<DependsOn, Position>>();

        var expectedId = new RelationComponentIdentifier(ecs.GetEntityForType<Position>().Id.ShortId, BuiltInRelationType.DependsOn);

        Assert.That(ecs.GetEntityForType<Relation<DependsOn, Position>>().Id.Value, Is.EqualTo(expectedId.Value));
    }

    [Test]
    public void QueryArchetypeCaching()
    {
        var ecs = World.Create();

        var entity = ecs.CreateEntity();
        entity.Add<Name>();
        ref var name = ref entity.GetMutable<Name>();
        name = new("SomeName");

        using var query = Query.FromQueryParam<ParamGroup<Name, Position>>(ecs);

        Assert.That(query.GetEntityIds(), Has.None.EqualTo(entity.Id));

        entity.Add<Position>();
        ref var position = ref entity.GetMutable<Position>();
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
        var component = ecs.CreateTag();
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
        var component = ecs.CreateTag();
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
        var component = ecs.CreateTag();
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
        _ = ecs.AddSystem((Identifier id, RefReadonly<Name> name, Ref<Position> position, Ref<Speed> speedRef) =>
        {
            ref var speed = ref speedRef.Reference;
            speed.X *= 2;
            speed.Y *= 2;
        });
        _ = ecs.AddSystem((Identifier id, Name name, Speed speed, Ref<Position> positionRef) =>
        {
            ref var position = ref positionRef.Reference;
            position.X += speed.X;
            position.Y += speed.Y;
        });
        _ = ecs.AddSystem((Identifier id, Name name, Ref<Position> positionRef) =>
        {
            ref var position = ref positionRef.Reference;
            if (name.Value == "WrongName")
            {
                position = new(50, 50);
            }
        });

        var someEntity1 = ecs.CreateEntity();
        someEntity1.Set<Name>(new("SomeName1"));
        someEntity1.Set<Position>(new(1, 1));
        someEntity1.Set<Speed>(new(1, 1));

        var someEntity2 = ecs.CreateEntity();
        someEntity2.Set<Name>(new("SomeName2"));
        someEntity2.Set<Position>(new(2, 2));
        someEntity2.Set<Speed>(new(2, 2));

        var someEntity3 = ecs.CreateEntity();
        someEntity3.Set<Name>(new("WrongName"));
        someEntity3.Set<Position>(new(3, 3));
        someEntity3.Set<Speed>(new(3, 3));

        ecs.Tick();

        Assert.That(someEntity1.Get<Position>(), Is.EqualTo(new Position(3, 3)));
        Assert.That(someEntity2.Get<Position>(), Is.EqualTo(new Position(6, 6)));
        Assert.That(someEntity3.Get<Position>(), Is.EqualTo(new Position(50, 50)));

        ecs.Tick();

        Assert.That(someEntity1.Get<Position>(), Is.EqualTo(new Position(7, 7)));
        Assert.That(someEntity2.Get<Position>(), Is.EqualTo(new Position(14, 14)));
        Assert.That(someEntity3.Get<Position>(), Is.EqualTo(new Position(50, 50)));
    }

    [Test]
    public void QueuedActions()
    {
        var ecs = World.Create();

        var entityQuery = Query.FromQueryParam<ParamGroup<Name, Position, Speed, int>>(ecs);

        _ = ecs.AddSystem((Identifier id, Name name, Position position, Speed speed, int i, EntityActions actions) =>
        {
            var entity = actions.CreateEntity($"Child of {name}");
            entity.Set(i);
            entity.Add<Position>();
            Assert.That(entity.Has<Position>(), Is.True);

            entity.Set(position);
            Assert.That(entity.Get<Position>(), Is.EqualTo(position));

            ref var mutablePosition = ref entity.GetMutable<Position>();
            mutablePosition.X += speed.X;
            mutablePosition.Y += speed.Y;

            Assert.That(entity.Get<Position>(), Is.EqualTo(mutablePosition));

            entity.Set(speed);
            Assert.That(entity.Has<Speed>(), Is.True);

            actions.Kill(id);
            Assert.That(actions.IsAlive(id), Is.False);
        });

        var someEntity1 = ecs.CreateEntity();
        someEntity1.Set<Name>(new("SomeName1"));
        someEntity1.Set<Position>(new(1, 1));
        someEntity1.Set<Speed>(new(1, 1));
        someEntity1.Set<int>(1);

        var someEntity2 = ecs.CreateEntity();
        someEntity2.Set<Name>(new("SomeName2"));
        someEntity2.Set<Position>(new(2, 2));
        someEntity2.Set<Speed>(new(2, 2));
        someEntity2.Set<int>(2);

        var someEntity3 = ecs.CreateEntity();
        someEntity3.Set<Name>(new("SomeName3"));
        someEntity3.Set<Position>(new(3, 3));
        someEntity3.Set<Speed>(new(3, 3));
        someEntity3.Set<int>(3);

        ecs.Tick();

        Assert.That(someEntity1.IsAlive(), Is.False);
        Assert.That(someEntity2.IsAlive(), Is.False);
        Assert.That(someEntity3.IsAlive(), Is.False);

        var childEntityEntries = entityQuery.GetEntityEntries().ToArray();
        Assert.That(childEntityEntries, Has.Length.EqualTo(3));

        foreach (var entry in childEntityEntries)
        {
            Assert.Multiple(() =>
            {
                var entity = new Entity(entry.Archetype.Entities[entry.Index], ecs);
                var i = entity.Get<int>();

                Assert.That(entity.Get<Name>().Value, Is.EqualTo($"Child of SomeName{i}"));
                Assert.That(entity.Get<Position>(), Is.EqualTo(new Position(i + i, i + i)));
                Assert.That(entity.Get<Speed>(), Is.EqualTo(new Speed(i, i)));
            });
        }
    }
}
