using System.Collections.Generic;
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
            return new SortedSetWithCreepyComparerCollection<T>();
        }

        public IMappedIntervalsCollection<T> Create<T>(IReadOnlyList<MappedInterval<T>> intervals)
        {
            throw new System.NotImplementedException();
        }
    }
}