using System;
using System.Collections.Generic;
using BenchmarkDotNet.Running;
using Contract;

namespace Console.Benchmarks
{
    internal sealed class Driver
    {
        private readonly ILogger _logger;
        private readonly IReadOnlyCollection<SandboxPlugin> _plugins;

        public Driver(ILogger logger, IReadOnlyCollection<SandboxPlugin> plugins)
        {
            _logger = logger;
            _plugins = plugins;
        }

        public void Run()
        {
            if (_plugins.Count == 0)
            {
                return;
            }

            try
            {
                foreach (var p in _plugins)
                {
                    _logger.Info(FormattableString.Invariant($"* Registered {p.Name} factory."));
                    CollectionBag.Register(new CollectionDescription(p));
                }
                _logger.Info("Running benchmarks...");

                BenchmarkRunner.Run<SinglePutScenarios<ValueCrate<int>>>();
                BenchmarkRunner.Run<SingleDeleteScenarios<ValueCrate<int>>>();
                BenchmarkRunner.Run<DeleteScenarios<ValueCrate<int>>>();

                BenchmarkRunner.Run<RealSinglePutAscendingScenario<ValueCrate<int>>>();
                BenchmarkRunner.Run<RealSinglePutDescendingScenario<ValueCrate<int>>>();
                BenchmarkRunner.Run<RealSinglePutRandomScenario<ValueCrate<int>>>();
                BenchmarkRunner.Run<RealSinglePutRandomAscendingSeriesScenario<ValueCrate<int>>>();
                BenchmarkRunner.Run<RealSinglePutRandomDescendingSeriesScenario<ValueCrate<int>>>();
                BenchmarkRunner.Run<RealSinglePutRandomBatchedSeriesScenario<ValueCrate<int>>>();

                BenchmarkRunner.Run<RealEnumerationAscendingScenario<ValueCrate<int>>>();
                BenchmarkRunner.Run<RealEnumerationDescendingScenario<ValueCrate<int>>>();
                BenchmarkRunner.Run<RealEnumerationRandomScenario<ValueCrate<int>>>();
            }
            finally
            {
                CollectionBag.Cleanup();
            }
        }
    }
}