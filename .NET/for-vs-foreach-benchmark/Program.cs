using BenchmarkDotNet.Running;

namespace for_vs_foreach_benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<CollectionsHolderBenchmark>();
        }
    }
}
