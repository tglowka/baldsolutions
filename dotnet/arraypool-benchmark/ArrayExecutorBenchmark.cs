// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class ArrayExecutorBenchmark
{
    private ArrayExecutor _arrayExecutor;

    [Params(10_000, 100_000, 1_000_000)]
    public int size;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _arrayExecutor = new ArrayExecutor(size);
    }

    [Benchmark]
    public void ArrayLoop() => _arrayExecutor.ArrayLoop();

    [Benchmark]
    public void ArrayPoolLoop() => _arrayExecutor.ArrayPoolLoop();
}