using async_eliding_part_1;
using BenchmarkDotNet.Attributes;
using System.Threading.Tasks;

namespace async_eliding_part_1_benchmark
{
    [MemoryDiagnoser]
    public class ExecutorBenchmarks
    {
        private readonly Executor _executor = new Executor();

        [Benchmark]
        public async Task<int> ExecuteAsyncAllTheWayDown() => await _executor.ExecuteAsyncAllTheWayDown();

        [Benchmark]
        public async Task<int> ExecuteAsyncEliding() => await _executor.ExecuteAsyncEliding();
    }
}
