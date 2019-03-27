using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
#if BENCHMARKING_OUTSIDE
using BenchmarkDotNet.Diagnosers;
#endif
using Contract;

namespace Console.Benchmarks
{
    //[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)] // https://github.com/dotnet/BenchmarkDotNet/issues/1109
    [HtmlExporter]
    //[MemoryDiagnoser]
#if BENCHMARKING_OUTSIDE
    //[HardwareCounters(HardwareCounter.CacheMisses, HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
#else
    [InProcess] // It is now run in-process only, as separate executable won't load plugins and fail.
#endif
    public abstract class RealSinglePutScenariosBase<TPayload> : CollectionBenchmarkBase<TPayload>
        where TPayload : new()
    {
        protected MappedInterval<TPayload>[] _ascending;
        protected MappedInterval<TPayload>[] _descending;
        protected MappedInterval<TPayload>[] _shuffled;
        protected Tuple<int, int>[] _ranges;

        [ParamsAllValues]
        public DataSource Source { get; set; }

        [ParamsAllValues]
        public DataFilter Filter { get; set; }

        [Params(1000)]
        public int Count { get; set; }

        protected override void AfterGlobalSetup()
        {
            var dummy = new TPayload();

            _ascending = new MappedInterval<TPayload>[Count];
            DataGeneration.LikeReal(Source, Filter, _ => dummy, _ascending);

            _descending = new MappedInterval<TPayload>[_ascending.Length];
            Reverse(_ascending, _descending);

            _shuffled = new MappedInterval<TPayload>[_ascending.Length];
            Shuffle(_ascending, _shuffled);

            var ranges = GeneratePartitions(_ascending.Length).ToArray();
            _ranges = new Tuple<int, int>[ranges.Length];
            Shuffle(ranges, _ranges);

            base.AfterGlobalSetup();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void AddSeries(IReadOnlyList<MappedInterval<TPayload>> input, IReadOnlyList<Tuple<int, int>> ranges)
        {
            var collection = Collection;
            var box = new MappedInterval<TPayload>[1];

            foreach (var range in ranges)
            {
                for (var i = range.Item1; i < range.Item2; ++i)
                {
                    box[0] = input[i];
                    collection.Put(box);
                }
            }
        }

        private static void Shuffle<T>(IReadOnlyList<T> input, T[] output)
        {
            Debug.Assert(input.Count == output.Length, "Must be matching.");
            var rand = new Random(0xC0FFEE);
            for (var i = 0; i < input.Count; ++i)
            {
                var j = rand.Next(0, i + 1);
                output[i] = output[j];
                output[j] = input[i];
            }
        }

        private static void Reverse<T>(IReadOnlyList<T> input, T[] output)
        {
            Debug.Assert(input.Count == output.Length, "Must be matching.");
            for (var i = 0; i < input.Count; ++i)
            {
                output[i] = input[input.Count - 1 - i];
            }
        }

        private static IEnumerable<Tuple<int, int>> GeneratePartitions(int count)
        {
            var r = new Random(0xDADB0B);
            var average = count < 16 ? count / 2 : count / 16;
            var current = 0;
            while (current < count)
            {
                var from = current;
                current = Math.Min(current + r.Next(average), count);
                yield return Tuple.Create(from, current);
            }
        }
    }
}