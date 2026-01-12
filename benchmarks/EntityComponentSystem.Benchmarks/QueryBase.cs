using System.ComponentModel;
using Tourmi.EntityComponentSystem.Components;

namespace Tourmi.EntityComponentSystem;

[InProcess]
[HideColumns("StdDev", "RatioSD")]
[MemoryDiagnoser]
public abstract class QueryBase
{
    public record struct Position(float X, float Y);
    public record struct Speed(float X, float Y);

    private protected static readonly QueryParamAction<ParamGroup<Ref<Position>, RefReadonly<Speed>>>[] ParamGroupSystems = [
        SimpleSystemParamGroup, ComplexSystemParamGroup,
    ];

    private protected World? _ecs;
    private protected Query? _query;

    private protected ArchetypeEntityEntry[]? _entityEntries;
    private protected Identifier[]? _entityIds;
    private protected Archetype[]? _archetypes;

    private protected Position[]? _dumbPositions;
    private protected Speed[]? _dumbSpeeds;

    private protected (Position, Speed)[]? _result;

    private protected Identifier _positionId;
    private protected Identifier _speedId;

    public int AdditionalComponentCount { get; set; } = 10;

    [Params(100, 100_000, Priority = 1)]
    public int EntityCount { get; set; } = 1;

    [Params(0, 1, Priority = 0)]
    public int SystemComplexity { get; set; } = 0;

    [GlobalSetup]
    public void GlobalSetup()
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

            // Make sure not all entities have the same archetype by randomly giving them components.
            while (random.NextSingle() > 0.5)
            {
                var componentId = additionalComponents[random.Next(AdditionalComponentCount)];
                if (!_ecs.Has(entity, componentId))
                {
                    _ecs.Add(entity, componentId);
                }
            }

            // Half of entities have a position
            if (random.NextSingle() > 0.5)
            {
                entity.Set<Position>(new(random.Next(-1000, 1000), random.Next(-1000, 1000)));
            }

            // Half of entities have a speed
            if (random.NextSingle() > 0.5)
            {
                entity.Set<Speed>(new(random.Next(-1000, 1000), random.Next(-1000, 1000)));
            }
        }

        _query = Query.FromQueryParam<ParamGroup<Position, Speed>>(_ecs);

        _entityEntries = _query.GetEntityEntries().ToArray();
        _entityIds = _entityEntries.Select(e => e.Archetype.Entities[e.Index]).ToArray();
        _archetypes = _query.GetArchetypes().ToArray();

        _dumbPositions = _entityEntries.Select(e => e.GetValue<Position>(_positionId)).ToArray();
        _dumbSpeeds = _entityEntries.Select(e => e.GetValue<Speed>(_speedId)).ToArray();

        _result = new (Position, Speed)[_entityIds.Length];
    }

    /// <summary>
    /// Function that ensures that the benchmarks don't get JITed to nothing.
    /// Sadly adds some overhead to all benchmarks, however.
    /// </summary>
    private protected (Position, Speed)[] CollectResults()
    {
        var entityIndex = 0;
        foreach (var archetype in _archetypes!)
        {
            var positionCollection = archetype.GetComponentCollection<Position>(_positionId).AsSpan();
            var speedCollection = archetype.GetComponentCollection<Speed>(_speedId).AsSpan();

            for (var i = 0; i < archetype.EntityCount; i++)
            {
                _result![entityIndex++] = (positionCollection[i], speedCollection[i]);
            }
        }

        return _result!;
    }

    private protected static void SimpleSystemParamGroup(ParamGroup<Ref<Position>, RefReadonly<Speed>> param)
    {
        (var positionRef, var speedRef) = param;
        ref var position = ref positionRef.Reference;
        ref readonly var speed = ref speedRef.Reference;

        position.X += speed.X;
        position.Y += speed.Y;
    }

    private protected static void ComplexSystemParamGroup(ParamGroup<Ref<Position>, RefReadonly<Speed>> param)
    {
        (var position, var speed) = param;

        var inc = 1;
        // bunch of interdependant non-sense calcs so that the content of the loop doesn't get short-circuited
        for (var i = 0; i < 100; i++)
        {
            inc += (int)position.Reference.X ^ (int)position.Reference.Y;
            position.Reference.X += 1 - inc % 3;
        }

        position.Reference.X += speed.Reference.X;
        position.Reference.Y += speed.Reference.Y;
    }

    private protected void ManualSystem(ref Position position, ref readonly Speed speed)
    {
        ParamGroupSystems[SystemComplexity](new(new(ref position), new(in speed)));
    }
}
