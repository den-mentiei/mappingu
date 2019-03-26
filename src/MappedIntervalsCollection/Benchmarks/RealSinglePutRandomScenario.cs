using System;
using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class RealSinglePutRandomScenario<TPayload> : RealSinglePutScenariosBase<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void Random()
        {
            var whole = new Tuple<int, int>[1];
            whole[0] = Tuple.Create(0, _shuffled.Length);
            AddSeries(_shuffled, whole);
        }
    }
}