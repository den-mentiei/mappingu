using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class SinglePutRandomDescendingSeries<TPayload> : SinglePutWithRealData<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void RandomDescendingSeries()
        {
            AddSeries(_descending, _ranges);
        }
    }
}