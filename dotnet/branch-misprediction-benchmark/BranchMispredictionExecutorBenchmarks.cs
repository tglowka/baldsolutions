// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

[HardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
public class BranchMispredictionExecutorBenchmarks
{
    private readonly BranchMispredictionExecutor _executor;
    private readonly int _size = 100000;

    public BranchMispredictionExecutorBenchmarks()
    {
        _executor = new BranchMispredictionExecutor(_size);
    }

    [Benchmark]
    public void SortedSumLessThan()
    {
        var sum = _executor.SortedSumLessThan(_size / 2);
    }

    [Benchmark]
    public void UnsortedSumLessThan()
    {
        var sum = _executor.UnsortedSumLessThan(_size / 2);
    }
}