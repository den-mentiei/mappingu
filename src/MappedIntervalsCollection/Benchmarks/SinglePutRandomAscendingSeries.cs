using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class SinglePutRandomAscendingSeries<TPayload> : SinglePutWithRealData<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void RandomAscendingSeries()
        {
            AddSeries(_ascending, _ranges);
        }
    }

}