using System;
using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class SinglePutDescending<TPayload> : SinglePutWithRealData<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void Descending()
        {
            var whole = new Tuple<int, int>[1];
            whole[0] = Tuple.Create(0, _descending.Length);
            AddSeries(_descending, whole);
        }
    }
}