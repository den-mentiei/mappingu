﻿using System.Collections.Generic;
using BenchmarkDotNet.Running;
using Contract;

namespace Console
{
    internal sealed class Benchmarks
    {
        private readonly IReadOnlyCollection<SandboxPlugin> _plugins;

        public Benchmarks(IReadOnlyCollection<SandboxPlugin> plugins)
        {
            _plugins = plugins;
        }

        public void Run()
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}