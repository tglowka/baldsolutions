using BenchmarkDotNet.Attributes;
using LINQ_vs_PLINQ;

namespace LINQ_vs_PLINQ_benchmark
{
    public class SelectExecutorBenchmarks
    {
        [Benchmark]
        [Arguments(10_000_000, 1)]
        [Arguments(10_000_000, 2)]
        [Arguments(10_000_000, 4)]
        [Arguments(10_000_000, 8)]
        [Arguments(10_000_000, 16)]
        public void SelectPLINQ(int size, int degree)
        {
            var executor = new SelectExecutor(size);
            executor.SelectPLINQ(degree);
        }

        [Benchmark]
        [Arguments(10_000_000)]
        public void SelectLINQ(int size)
        {
            var executor = new SelectExecutor(size);
            executor.SelectLINQ();
        }
    }
}
