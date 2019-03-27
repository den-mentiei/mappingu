using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class RealEnumerationAscendingScenario<TPayload> : RealEnumerationScenariosBase<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void Work()
        {
            Execute(_ascendingOffsets);
        }
    }
}