using System;
using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class RealSinglePutAscendingScenario<TPayload> : RealSinglePutScenariosBase<TPayload>
        where TPayload : new()
    {
        [Benchmark(Baseline = true)]
        public void Ascending()
        {
            var whole = new Tuple<int, int>[1];
            whole[0] = Tuple.Create(0, _ascending.Length);
            AddSeries(_ascending, whole);
        }
    }
}