using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class EnumerationRandom<TPayload> : EnumerationWithRealData<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void Work()
        {
            Execute(_randomOffsets);
        }
    }
}