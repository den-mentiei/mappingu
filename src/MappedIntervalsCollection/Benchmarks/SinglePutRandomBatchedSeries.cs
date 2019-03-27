using System;
using BenchmarkDotNet.Attributes;
using Contract;

namespace Console.Benchmarks
{
    public class SinglePutRandomBatchedSeries<TPayload> : SinglePutWithRealData<TPayload>
        where TPayload : new()
    {
        [Benchmark]
        public void RandomBatchedSeries()
        {
            var collection = Collection;
            foreach (var range in _ranges)
            {
                collection.Put(new ArraySegment<MappedInterval<TPayload>>(_ascending, range.Item1, range.Item2 - range.Item1));
            }
        }
    }
}