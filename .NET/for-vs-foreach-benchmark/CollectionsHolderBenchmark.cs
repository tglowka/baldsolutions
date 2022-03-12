using BenchmarkDotNet.Attributes;
using for_vs_foreach;

namespace for_vs_foreach_benchmark
{
    public class CollectionsHolderBenchmark
    {
        private readonly CollectionsHolder _collectionsHolder = new CollectionsHolder(1_000_000_000);

        [Benchmark]
        public void ForLoopList() => _collectionsHolder.ForLoopList();

        [Benchmark]
        public void ForeachLoopList() => _collectionsHolder.ForeachLoopList();

        [Benchmark]
        public void ForLoopArray() => _collectionsHolder.ForLoopArray();

        [Benchmark]
        public void ForeachLoopArray() => _collectionsHolder.ForeachLoopArray();
    }
}
