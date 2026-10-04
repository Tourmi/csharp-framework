namespace Tourmi.EntityComponentSystem;

public class QueryForEachVsDynamic : QueryBase
{
    private static readonly Action<ParamGroup<Ref<Position>, RefReadonly<Speed>>>[] DynamicParamGroupSystems = [
        SimpleSystemParamGroup, ComplexSystemParamGroup,
    ];
    private static readonly QueryParamAction<Ref<Position>, RefReadonly<Speed>>[] StaticSystems = [
        SimpleSystem, ComplexSystem,
    ];
    private static readonly Action<Ref<Position>, RefReadonly<Speed>>[] DynamicSystems = [
        SimpleSystem, ComplexSystem,
    ];

    [Benchmark(Baseline = true)]
    public void ForEachStaticParamGroup()
    {
        _query!.ForEach(ParamGroupSystems[SystemComplexity]);
    }

    [Benchmark]
    public void ForEachDynamicParamGroup()
    {
        _query!.ForEach(DynamicParamGroupSystems[SystemComplexity]);
    }

    [Benchmark]
    public void ForEachDynamic()
    {
        _query!.ForEach(DynamicSystems[SystemComplexity]);
    }

    [Benchmark]
    public void ForEachStatic()
    {
        _query!.ForEach(StaticSystems[SystemComplexity]);
    }

    private static void SimpleSystem(Ref<Position> positionRef, RefReadonly<Speed> speedRef)
    {
        ref var position = ref positionRef.Reference;
        ref readonly var speed = ref speedRef.Reference;

        position.X += speed.X;
        position.Y += speed.Y;
    }

    private static void ComplexSystem(Ref<Position> position, RefReadonly<Speed> speed)
    {
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
}
