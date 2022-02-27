using Add_vs_AddRange;
using BenchmarkDotNet.Attributes;
using System.Linq;

namespace Add_vs_AddRange_benchmark
{
    public class ListExecutorBenchmarks
    {
        [Benchmark]
        [Arguments(10_000, 1_000_000)]
        [Arguments(10_000, 10_000_000)]
        [Arguments(10_000, 100_000_000)]
        [Arguments(10_000, 1_000_000_000)]
        public void AddOnebyOne(int listSize, int listToAddSize)
        {
            var executor = new ListExecutor(listSize);

            var toAdd = Enumerable.Range(0, listToAddSize).ToList();

            executor.AddOnebyOne(toAdd);
        }

        [Benchmark]
        [Arguments(10_000, 1_000_000)]
        [Arguments(10_000, 10_000_000)]
        [Arguments(10_000, 100_000_000)]
        [Arguments(10_000, 1_000_000_000)]
        public void AddRange(int listSize, int listToAddSize)
        {
            var executor = new ListExecutor(listSize);

            var toAdd = Enumerable.Range(0, listToAddSize).ToList();

            executor.AddRange(toAdd);
        }
    }
}
