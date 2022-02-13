using BenchmarkDotNet.Attributes;
using data_locality;

namespace data_locality_benchmark
{
    public class MultidimensionalArrayExperimentBenchmarks
    {
        [Benchmark]
        [Arguments(100)]
        [Arguments(1000)]
        [Arguments(10000)]
        public void IterateWriteSequential(int size)
        {
            var _arrayExperiment = new MultidimensionalArrayExperiment(size);
            _arrayExperiment.IterateWriteSequential();
        }

        [Benchmark]
        [Arguments(100)]
        [Arguments(1000)]
        [Arguments(10000)]
        public void IterateWriteNonSequential(int size)
        {
            var _arrayExperiment = new MultidimensionalArrayExperiment(size);
            _arrayExperiment.IterateWriteNonSequential();
        }
    }
}
