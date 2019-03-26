using Contract;

namespace AvlTreePlugin
{
    internal sealed class AvlTreePluginFactory : SandboxPlugin
    {
        public AvlTreePluginFactory(ILogger logger)
            : base(logger)
        {
        }

        public override string Name => nameof(AvlTreePluginFactory);

        public override IMappedIntervalsCollection<T> CreateCollection<T>()
        {
            return new MappedIntervalTree<T>(new MappedIntervalComparer<T>());
        }
    }
}
