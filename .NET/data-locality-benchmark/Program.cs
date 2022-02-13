using BenchmarkDotNet.Running;

namespace data_locality_benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MultidimensionalArrayExperimentBenchmarks>();
            BenchmarkRunner.Run<JaggedArrayExperimentBenchmarks>();
        }
    }
}
