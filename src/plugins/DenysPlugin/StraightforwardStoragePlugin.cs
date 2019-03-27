using System.Collections.Generic;
using Contract;

namespace DenysPlugin
{
    public sealed class StraightforwardStoragePlugin : SandboxPlugin, IStorageFactory
    {
        public StraightforwardStoragePlugin(ILogger logger)
            : base(logger)
        {
        }

        public override string Name => nameof(StraightforwardStoragePlugin);

        public override IStorageFactory Factory => this;

        public IMappedIntervalsCollection<T> Create<T>()
        {
            return new StraightforwardStorage<T>();
        }

        public IMappedIntervalsCollection<T> Create<T>(IReadOnlyList<MappedInterval<T>> intervals)
        {
            throw new System.NotImplementedException();
        }
    }
}
