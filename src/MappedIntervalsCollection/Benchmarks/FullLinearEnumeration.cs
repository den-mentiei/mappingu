using BenchmarkDotNet.Attributes;

namespace Console.Benchmarks
{
    public class FullLinearEnumeration<TPayload> : EnumerationWithRealData<TPayload>
        where TPayload : new()
    {
        [Benchmark(Baseline = true)]
        public void PlainArray()
        {
            foreach (var i in _data)
            {
                _consumer.Consume(i.IntervalStart);
            }
        }

        [Benchmark]
        public void Work()
        {
            foreach (var i in Collection)
            {
                _consumer.Consume(i.IntervalStart);
            }
        }
    }
}