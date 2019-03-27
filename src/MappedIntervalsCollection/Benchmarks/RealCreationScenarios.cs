using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace Console.Benchmarks
{
    public class RealCreationScenarios<TPayload> : RealSinglePutScenariosBase<TPayload>
        where TPayload : new()
    {
        private Consumer _consumer;

        protected override void AfterGlobalSetup()
        {
            _consumer = new Consumer();
            base.AfterGlobalSetup();
        }

        [Benchmark]
        public void Work()
        {
            Description.Create(_ascending).Consume(_consumer);
        }
    }
}