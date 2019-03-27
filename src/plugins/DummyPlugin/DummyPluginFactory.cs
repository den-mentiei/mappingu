using System;
using System.Collections.Generic;
using System.Linq;
using Contract;

namespace DummyPlugin
{
    public sealed class DummyPlugin : SandboxPlugin, IStorageFactory
    {
        public DummyPlugin(ILogger logger)
            : base(logger)
        {
        }

        public override string Name => "SortedSetWithCreepyComparer";

        public override IStorageFactory Factory => this;

        public IMappedIntervalsCollection<T> Create<T>()
        {
            return Create(Array.Empty<MappedInterval<T>>());
        }

        public IMappedIntervalsCollection<T> Create<T>(IReadOnlyList<MappedInterval<T>> intervals)
        {
            return new SortedSetWithCreepyComparerCollection<T>(intervals);
        }
    }
}