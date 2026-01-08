namespace Tourmi.EntityComponentSystem;

[GenericTypeArguments(typeof(EmptyStruct))]
[GenericTypeArguments(typeof(byte))]
[GenericTypeArguments(typeof(short))]
[GenericTypeArguments(typeof(int))]
[GenericTypeArguments(typeof(long))]
[GenericTypeArguments(typeof((long, long, long, long, long, long, long, long)))]
[InProcess]
[ShortRunJob]
[HideColumns("StdDev", "RatioSD")]
public class EntityIdentifiersGet<T>
{
    public readonly record struct StructComponent(T Value);

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
                    _entityCollection.AddComponent(entity, component, typeof(StructComponent));
                }
            }
        }

        _entity = _entities[random.Next(EntityCount)];

        if (!_entityCollection.HasComponent(_entity, _component))
        {
            _entityCollection.AddComponent(_entity, _component, typeof(StructComponent));
        }
    }

    [Benchmark]
    public string? Get()
    {
        return _entityCollection.GetComponent<StructComponent>(_entity, _component).Value?.ToString();
    }

    [Benchmark]
    public string? GetRef()
    {
        return _entityCollection.GetRefComponent<StructComponent>(_entity, _component).Value?.ToString();
    }

    private Identifier Create()
    {
        var id = _idCollection.Create();
        _entityCollection.Create(id);
        return id;
    }
}
