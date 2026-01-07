using System.ComponentModel;
using Tourmi.EntityComponentSystem.Components;

namespace Tourmi.EntityComponentSystem;

[InProcess]
public class QueryForEach
{
    public record struct Position(float X, float Y);
    public record struct Speed(float X, float Y);

    private static readonly Action<ParamGroup<Ref<Position>, RefReadonly<Speed>>>[] Systems = [
        NoOpSystem, SimpleSystem, AdvancedSystem,
    ];

    private World? _ecs;
    private Query? _query;

    private ArchetypeEntityEntry[]? _entityEntries;
    private Identifier[]? _entityIds;
    private Archetype[]? _archetypes;

    private Position[]? _dumbPositions;
    private Speed[]? _dumbSpeeds;

    private Identifier _positionId;
    private Identifier _speedId;

    public int AdditionalComponentCount { get; set; } = 10;

    [Params(100, 100_000, Priority = 1)]
    public int EntityCount { get; set; } = 1;

    [Params(0, 1, 2, Priority = 0)]
    public int SystemComplexity {get; set;} = 0;

    [GlobalSetup]
    public void Setup()
    {
        _ecs = World.Create();
        var random = new Random(0);

        var additionalComponents = new Identifier[AdditionalComponentCount];
        for (var i = 0; i < AdditionalComponentCount; i++)
        {
            var entity = _ecs.CreateEntity();
            entity.Add<Component>();
            entity.Set<Name>(new($"Additional Component {i}"));
            additionalComponents[i] = entity;
        }

        _positionId = _ecs.GetComponentForType<Position>();
        _speedId = _ecs.GetComponentForType<Speed>();

        for (var i = 0; i < EntityCount; i++)
        {
            var entity = _ecs.CreateEntity();

            while (random.NextSingle() > 0.5)
            {
                var componentId = additionalComponents[random.Next(AdditionalComponentCount)];
                if (!_ecs.Has(entity, componentId))
                {
                    _ecs.Add(entity, componentId);
                }
            }

            if ((i % 4) is 0 or 1)
            {
                entity.Set<Position>(new(random.Next(-1000, 1000), random.Next(-1000, 1000)));
            }

            if ((i % 4) is 1 or 3)
            {
                entity.Set<Speed>(new(random.Next(-1000, 1000), random.Next(-1000, 1000)));
            }
        }

        _query = new Query(_ecs, _positionId, _speedId);

        _entityEntries = _query.GetEntityEntries().ToArray();
        _entityIds = _entityEntries.Select(e => e.Archetype.Entities[e.Index]).ToArray();
        _archetypes = _query.GetArchetypes().ToArray();

        _dumbPositions = _entityEntries.Select(e => e.GetValue<Position>(_positionId)).ToArray();
        _dumbSpeeds = _entityEntries.Select(e => e.GetValue<Speed>(_speedId)).ToArray();
    }

    [Benchmark]
    public (Position, Speed)[] DumbFlatArrays()
    {
        for (var i = 0; i < _dumbPositions!.Length; i++)
        {
            ManualSystem(ref _dumbPositions[i], ref _dumbSpeeds![i]);
        }

        return _dumbPositions.Zip(_dumbSpeeds!).ToArray();
    }

    [Benchmark]
    public (Position, Speed)[] ManualComponentCollections()
    {
        foreach (var archetype in _archetypes!)
        {
            var positionCollection = archetype.GetComponentCollection<Position>(_positionId);
            var speedCollection = archetype.GetComponentCollection<Speed>(_speedId);

            for (var i = 0; i < archetype.EntityCount; i++)
            {
                ManualSystem(ref positionCollection[i], in speedCollection[i]);
            }
        }

        return _entityEntries!.Select(e => (e.GetValue<Position>(_positionId), e.GetValue<Speed>(_speedId))).ToArray();
    }

    [Benchmark]
    public (Position, Speed)[] ManualArchetypeEntry()
    {
        foreach (var entry in _entityEntries!)
        {
            ManualSystem(ref entry.GetValueRef<Position>(_positionId), in entry.GetValueRef<Speed>(_speedId));
        }

        return _entityEntries!.Select(e => (e.GetValue<Position>(_positionId), e.GetValue<Speed>(_speedId))).ToArray();
    }

    [Benchmark]
    public (Position, Speed)[] ManualEntities()
    {
        foreach (var id in _entityIds!)
        {
            var entity = new Entity(id, _ecs);
            ManualSystem(ref entity.GetMutable<Position>(_positionId), in entity.GetMutable<Speed>(_speedId));
        }
        
        return _entityEntries!.Select(e => (e.GetValue<Position>(_positionId), e.GetValue<Speed>(_speedId))).ToArray();
    }

    [Benchmark(Baseline = true)]
    public (Position, Speed)[] ParamGroupForEach()
    {
        _query!.ForEach(Systems[SystemComplexity]);
        return _entityEntries!.Select(e => (e.GetValue<Position>(_positionId), e.GetValue<Speed>(_speedId))).ToArray();
    }

    private static void NoOpSystem(ParamGroup<Ref<Position>, RefReadonly<Speed>> param)
    {
    }

    private static void SimpleSystem(ParamGroup<Ref<Position>, RefReadonly<Speed>> param)
    {
        (var positionRef, var speedRef) = param;
        ref var position = ref positionRef.Reference;
        ref readonly var speed = ref speedRef.Reference;

        position.X += speed.X;
        position.Y += speed.Y;
    }

    private static void AdvancedSystem(ParamGroup<Ref<Position>, RefReadonly<Speed>> param)
    {
        (var position, var speed) = param;

        if (position.Reference.X < 0)
        {
            position.Reference.X *= -1;
        }

        if (position.Reference.Y < 0)
        {
            position.Reference.Y *= -1;
        }

        if (speed.Reference.X > 0)
        {
            position.Reference.X += 1;
            position.Reference.Y += 1;
        }

        if (speed.Reference.Y < 0)
        {
            position.Reference.X -= 1;
            position.Reference.Y -= 1;
        }

        var inc = 1;
        for(var i = 0; i < 100; i++)
        {
            inc += (int)position.Reference.X ^ (int)position.Reference.Y;
            position.Reference.X += 1 - inc % 3;
        }

        position.Reference.X += speed.Reference.X;
        position.Reference.Y += speed.Reference.Y;
    }

    private void ManualSystem(ref Position position, ref readonly Speed speed)
    {
        Systems[SystemComplexity](new(new(ref position), new(in speed)));
    }
}
