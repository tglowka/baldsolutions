using BenchmarkDotNet.Running;

namespace Add_vs_AddRange_benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ListExecutorBenchmarks>();
        }
    }
}
