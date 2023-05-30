// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class StackallocExecutorBenchmarks
{
    [Benchmark]
    [Arguments(100)]
    [Arguments(1_000)]
    [Arguments(10_000)]
    public void Stackalloc(int size)
    {
        var stacallocExecutor = new StackallocExecutor(size);
        stacallocExecutor.Stackalloc();
    }

    [Benchmark]
    [Arguments(100)]
    [Arguments(1_000)]
    [Arguments(10_000)]
    public void NoStackalloc(int size)
    {
        var stacallocExecutor = new StackallocExecutor(size);
        stacallocExecutor.NoStackalloc();
    }
}