using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class RealSinglePutRandomAscendingSeriesScenario<TPayload> : RealSinglePutScenariosBase<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void RandomAscendingSeries()
        {
            AddSeries(_ascending, _ranges);
        }
    }

}