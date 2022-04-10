// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

[HardwareCounters(HardwareCounter.CacheMisses)]
public class FalseSharingExecutorBenchmark
{
    [Benchmark]
    public async Task ExecuteWithFalseSharing()
    {
        var falseSharingExecutor = new FalseSharingExecutor(1_000_000);
        await falseSharingExecutor.ExecuteWithFalseSharing();
    }

    [Benchmark]
    public async Task ExecuteFalseSharingWithImprovement()
    {
        var falseSharingExecutor = new FalseSharingExecutor(1_000_000);
        await falseSharingExecutor.ExecuteFalseSharingWithImprovement();
    }

    [Benchmark]
    public async Task ExecuteFalseSharingWithBetterImprovement()
    {
        var falseSharingExecutor = new FalseSharingExecutor(1_000_000);
        await falseSharingExecutor.ExecuteFalseSharingWithBetterImprovement();
    }
}
