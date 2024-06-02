// See https://aka.ms/new-console-template for more information

using System.Numerics;
using baby_step_giant_step;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Benchmarks>();

[MemoryDiagnoser]
public class Benchmarks
{
    private readonly BigInteger _mod = BigInteger.Parse("5210644015679228794060694325390955853335898483908056458352183851018372555735221");
    private readonly BigInteger _base = BigInteger.Parse("598156793758099511761753839885471060009647209443751599122360246998383254800634");
    private readonly BigInteger _result = BigInteger.Parse("3522197356311839067406761763558521917536353254804586391157901835191894911474192");
    private readonly long _limit = 855649200;

    [Benchmark]
    public void BabyStepGiantStep() => BabyStepGianStepAlgorithm.Compute(_base, _result, _mod, _limit);

    [Benchmark]
    public void BruteForce() => BruteForceAlgorithm.Compute(_base, _result, _mod, _limit);
}