using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
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
    public abstract class EnumerationWithRealData<TPayload> : CollectionBenchmarkBase<TPayload>
        where TPayload : new()
    {
        protected Consumer _consumer;
        protected MappedInterval<TPayload>[] _data;
        protected int[] _ascendingOffsets;
        protected int[] _descendingOffsets;
        protected int[] _randomOffsets;

        [ParamsAllValues]
        public DataSource Source { get; set; }

        [ParamsAllValues]
        public DataFilter Filter { get; set; }

        [Params(1000)]
        public int Count { get; set; }

        [Params(16)]
        public int ItemsPerOffset { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            _consumer = new Consumer();

            var dummy = new TPayload();

            _data = new MappedInterval<TPayload>[Count];
            DataGeneration.LikeReal(Source, Filter, _ => dummy, _data);

            var offsetCount = _data.Length / ItemsPerOffset;

            var span = _data.Length / offsetCount;
            _ascendingOffsets = new int[offsetCount];            
            _descendingOffsets = new int[offsetCount];
            for (var i = 0; i < _ascendingOffsets.Length; ++i)
            {
                _ascendingOffsets[i] = i * span;
                _descendingOffsets[_ascendingOffsets.Length - i - 1] = i * span;
            }

            var r = new Random(0xF00D);
            _randomOffsets = new int[offsetCount];
            for (var i = 0; i < _randomOffsets.Length; ++i)
            {
                _randomOffsets[i] = r.Next(0, _data.Length);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Execute(int[] offsets)
        {
            for (var i = 0; i < offsets.Length; ++i)
            {
                using (var e = Collection.GetEnumerator(offsets[i]))
                {
                    e.MoveNext();
                    _consumer.Consume(e.Current.IntervalStart);
                }
            }
        }

        protected override void AfterCollectionCreation()
        {
            Collection.Put(_data);
            base.AfterCollectionCreation();
        }
    }
}