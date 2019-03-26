using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class RealSinglePutRandomDescendingSeriesScenario<TPayload> : RealSinglePutScenariosBase<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void RandomDescendingSeries()
        {
            AddSeries(_descending, _ranges);
        }
    }
}