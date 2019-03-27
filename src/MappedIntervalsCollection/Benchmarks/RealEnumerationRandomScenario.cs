using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class RealEnumerationRandomScenario<TPayload> : RealEnumerationScenariosBase<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void Work()
        {
            Execute(_randomOffsets);
        }
    }
}