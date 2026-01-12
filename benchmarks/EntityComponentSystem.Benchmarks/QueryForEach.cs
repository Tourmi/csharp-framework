namespace Tourmi.EntityComponentSystem;

public class QueryForEach : QueryBase
{
    /// <summary>
    /// Best case benchmark. 
    /// Shows the unbeatable perf score, where we just have flat arrays for components.
    /// </summary>
    [Benchmark]
    public (Position, Speed)[] DumbFlatArrays()
    {
        for (var i = 0; i < _dumbPositions!.Length; i++)
        {
            ManualSystem(ref _dumbPositions[i], ref _dumbSpeeds![i]);
        }

        for (var i = 0; i < _entityEntries!.Length; i++)
        {
            _result![i] = (_dumbPositions[i], _dumbSpeeds![i]);
        }

        return _result!;
    }

    /// <summary>
    /// Benchmark that basically does what <see cref="Query.ForEach{T}(Action{T})"/> does,
    /// without the ParamGroup overhead.
    /// </summary>
    [Benchmark]
    public (Position, Speed)[] ManualComponentCollections()
    {
        var positionId = _ecs!.GetComponentForType<Position>();
        var speedId = _ecs!.GetComponentForType<Speed>();
        foreach (var archetype in _archetypes!)
        {
            var positionCollection = archetype.GetComponentCollection<Position>(positionId).AsSpan();
            var speedCollection = archetype.GetComponentCollection<Speed>(speedId).AsSpan();

            for (var i = 0; i < archetype.EntityCount; i++)
            {
                ManualSystem(ref positionCollection[i], in speedCollection[i]);
            }
        }

        return CollectResults();
    }

    /// <summary>
    /// Baseline, represents the current performance of using <see cref="Query.ForEach{T}(Action{T})"/>
    /// </summary>
    [Benchmark(Baseline = true)]
    public (Position, Speed)[] ForEach()
    {
        _query!.ForEach(ParamGroupSystems[SystemComplexity]);
        return CollectResults();
    }

    /// <summary>
    /// Benchmark where we iterate the entities, but fetch their data directly from the archetype for each entity.
    /// Should be slightly slower than <see cref="Query.ForEach{T}(Action{T})"/>,
    /// assuming that per-archetype cache (ie: caching the collection to iterate over) DOES improve performance.
    /// </summary>
    [Benchmark]
    public (Position, Speed)[] ManualArchetypeEntry()
    {
        foreach (var entry in _entityEntries!)
        {
            ManualSystem(ref entry.GetValueRef<Position>(_positionId), in entry.GetValueRef<Speed>(_speedId));
        }

        return CollectResults();
    }

    /// <summary>
    /// Worst case benchmark, where we naively do all operations through an entity.
    /// </summary>
    [Benchmark]
    public (Position, Speed)[] ManualEntities()
    {
        foreach (var id in _entityIds!)
        {
            var entity = new Entity(id, _ecs);
            ManualSystem(ref entity.GetMutable<Position>(_positionId), in entity.GetMutable<Speed>(_speedId));
        }

        return CollectResults();
    }
}
