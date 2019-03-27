using System.Collections.Generic;
using Contract;

namespace AvlTreePlugin
{
    internal sealed class AvlTreePlugin : SandboxPlugin, IStorageFactory
    {
        public AvlTreePlugin(ILogger logger)
            : base(logger)
        {
        }

        public override string Name => nameof(AvlTreePlugin);

        public override IStorageFactory Factory => this;

        public IMappedIntervalsCollection<T> Create<T>()
        {
            return new MappedIntervalTree<T>(new MappedIntervalComparer<T>());
        }

        public IMappedIntervalsCollection<T> Create<T>(IReadOnlyList<MappedInterval<T>> intervals)
        {
            throw new System.NotImplementedException();
        }
    }
}
