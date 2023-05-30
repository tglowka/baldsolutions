// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<CacheBenchmarks>();

public class CacheBenchmarks
{
    private readonly List<string> _keys = new();

    [GlobalSetup]
    public async Task Before()
    {
        for (int i = 0; i < 50; i++)
            _keys.Add(await Client.CreatePost());
    }

    [Benchmark]
    public async Task GetFromInMemory()
    {
        foreach (var key in _keys)
            await Client.GetPost(key, true, false);
    }

    [Benchmark]
    public async Task GetFromRedisCache()
    {
        foreach (var key in _keys)
            await Client.GetPost(key, false, true);
    }

    [Benchmark]
    public async Task GetFromPostgres()
    {
        foreach (var key in _keys)
            await Client.GetPost(key, false, false);
    }
}
