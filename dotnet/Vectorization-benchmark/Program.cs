// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<VectorizationBenchmarks>();


[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net60)]
[SimpleJob(RuntimeMoniker.Net70)]
public class VectorizationBenchmarks
{
    [Benchmark]
    [ArgumentsSource(nameof(Arrays))]
    public void Max(IEnumerable<int> array) => array.Max();

    [Benchmark]
    [ArgumentsSource(nameof(Arrays))]
    public void Max_Vectorized(IEnumerable<int> array) => array.Max_Vectorized();

    public IEnumerable<object> Arrays()
    {
        yield return Enumerable.Repeat(1, 1_00).ToArray();
        yield return Enumerable.Repeat(1, 10_000).ToArray();
        yield return Enumerable.Repeat(1, 1_000_000).ToArray();
    }
}