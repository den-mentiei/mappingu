using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class EnumerationDescending<TPayload> : EnumerationWithRealData<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void Work()
        {
            Execute(_descendingOffsets);
        }
    }
}