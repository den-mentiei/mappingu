using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class RealEnumerationDescendingScenario<TPayload> : RealEnumerationScenariosBase<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void Work()
        {
            Execute(_descendingOffsets);
        }
    }
}