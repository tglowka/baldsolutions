using BenchmarkDotNet.Running;

namespace LINQ_vs_PLINQ_benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SelectExecutorBenchmarks>();
        }
    }
}
