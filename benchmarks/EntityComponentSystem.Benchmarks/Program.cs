using BenchmarkDotNet.Running;

namespace Tourmi.EntityComponentSystem;

public static class Program
{
    public static void Main(string[] args)
    {
        // _ = BenchmarkSwitcher.FromTypes([typeof(EntityIdentifiersGet<>)]).RunAllJoined();
        // _ = BenchmarkSwitcher.FromTypes([typeof(EntityIdentifiersSet)]).RunAllJoined();
        // _ = BenchmarkSwitcher.FromTypes([typeof(QueryForEach)]).RunAllJoined();
        // _ = BenchmarkSwitcher.FromTypes([typeof(QueryForEachVsDynamic)]).RunAllJoined();
        _ = BenchmarkSwitcher.FromTypes([typeof(Systems)]).RunAllJoined();
    }
}
