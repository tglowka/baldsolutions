using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

//BenchmarkRunner.Run<RecursionBenchmark>();

//Both throw StackOverflowException
Console.WriteLine(Recursion.StandardRecursion(30000));
Console.WriteLine(Recursion.TailRecursion(30000));

public static class Recursion
{
    public static int TailRecursion(int x, int acc = 1) =>
        x == 0
            ? acc
            : TailRecursion(x - 1, acc * x);

    public static int StandardRecursion(int x) =>
        x < 1
            ? 1
            : x * StandardRecursion(x - 1);
}

//[MemoryDiagnoser]
//public class RecursionBenchmark
//{
//    [Params(10_000, 20_000, 30_000)]
//    public int size;

//    [Benchmark]
//    public void StandardRecursion() => Recursion.StandardRecursion(size);

//    [Benchmark]
//    public void TailRecursion() => Recursion.TailRecursion(size);
//}