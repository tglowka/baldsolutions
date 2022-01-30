using BenchmarkDotNet.Running;

namespace async_eliding_part_1_benchmark
{
    class Program
    {
        public static void Main() => BenchmarkRunner.Run<ExecutorBenchmarks>();
    }
}
