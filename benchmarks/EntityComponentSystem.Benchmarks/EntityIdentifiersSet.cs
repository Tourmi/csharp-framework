using Tourmi.EntityComponentSystem.Archetypes;
using Tourmi.EntityComponentSystem.Ids;

namespace Tourmi.EntityComponentSystem;

[MValueColumn]
[InProcess]
public class EntityIdentifiersSet
{
    public record struct Position(int X, int Y);

    private readonly IdentifierCollection _idCollection = new();
    private readonly EntityArchetypeCollection _entityCollection = new();
    private Identifier[]? _components;
    private Identifier[]? _entities;
    private Identifier _component;
    private Identifier _entity;

    [Params(100)]
    public int ComponentCount { get; set; } = 1;

    [Params(10_000)]
    public int EntityCount { get; set; } = 1;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(0);
        _components = new Identifier[ComponentCount];
        _entities = new Identifier[EntityCount];

        for (var i = 0; i < ComponentCount; i++)
        {
            _components[i] = Create();
        }

        _component = _components[random.Next(ComponentCount)];

        for (var i = 0; i < EntityCount; i++)
        {
            var entity = Create();
            _entities[i] = entity;
            while (random.NextDouble() > 0.25)
            {
                var component = _components[random.Next(ComponentCount)];
                if (!_entityCollection.HasComponent(entity, component))
                {
                    _entityCollection.AddComponent(entity, component, typeof(Position));
                }
            }
        }

        _entity = _entities[random.Next(EntityCount)];

        if (!_entityCollection.HasComponent(_entity, _component))
        {
            _entityCollection.AddComponent(_entity, _component, typeof(Position));
            _entityCollection.SetComponent<Position>(_entity, _component, new(1, 1));
        }
    }

    [Benchmark]
    public Position GetSet()
    {
        var component = _entityCollection.GetComponent<Position>(_entity, _component);
        component.X *= 2;
        component.Y *= 2;
        _entityCollection.SetComponent(_entity, _component, component);

        return _entityCollection.GetComponent<Position>(_entity, _component);
    }

    [Benchmark]
    public Position Set()
    {
        var component = new Position(1, 1);
        component.X *= 2;
        component.Y *= 2;
        _entityCollection.SetComponent(_entity, _component, component);

        return _entityCollection.GetComponent<Position>(_entity, _component);
    }

    [Benchmark]
    public Position SetRef()
    {
        ref var component = ref _entityCollection.GetRefComponent<Position>(_entity, _component);
        component = new(component.X * 2, component.Y * 2);

        return _entityCollection.GetComponent<Position>(_entity, _component);
    }

    [Benchmark]
    public Position MutateRef()
    {
        ref var component = ref _entityCollection.GetRefComponent<Position>(_entity, _component);
        component.X *= 2;
        component.Y *= 2;

        return _entityCollection.GetComponent<Position>(_entity, _component);
    }

    private Identifier Create()
    {
        var id = _idCollection.Create();
        _entityCollection.Create(id);
        return id;
    }
}
