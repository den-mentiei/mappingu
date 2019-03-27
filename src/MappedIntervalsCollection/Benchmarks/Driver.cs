﻿using System;
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

                /*BenchmarkRunner.Run<SinglePutScenarios<ValueCrate<int>>>();
                BenchmarkRunner.Run<SingleDeleteScenarios<ValueCrate<int>>>();
                BenchmarkRunner.Run<DeleteScenarios<ValueCrate<int>>>();*/

                BenchmarkRunner.Run<CreationFromData<ValueCrate<int>>>();

                BenchmarkRunner.Run<SinglePutAscending<ValueCrate<int>>>();
                BenchmarkRunner.Run<SinglePutDescending<ValueCrate<int>>>();
                BenchmarkRunner.Run<SinglePutRandom<ValueCrate<int>>>();
                BenchmarkRunner.Run<SinglePutRandomAscendingSeries<ValueCrate<int>>>();
                BenchmarkRunner.Run<SinglePutRandomDescendingSeries<ValueCrate<int>>>();
                BenchmarkRunner.Run<SinglePutRandomBatchedSeries<ValueCrate<int>>>();

                BenchmarkRunner.Run<FullLinearEnumeration<ValueCrate<int>>>();
                BenchmarkRunner.Run<EnumerationAscending<ValueCrate<int>>>();
                BenchmarkRunner.Run<EnumerationDescending<ValueCrate<int>>>();
                BenchmarkRunner.Run<EnumerationRandom<ValueCrate<int>>>();
            }
            finally
            {
                CollectionBag.Cleanup();
            }
        }
    }
}