using System.Numerics;

namespace baby_step_giant_step;

public static class BabyStepGianStepAlgorithm
{
    public static BigInteger Compute(BigInteger @base, BigInteger result, BigInteger mod, long limit)
    {
        var m = (long)Math.Ceiling(Math.Sqrt(limit));

        var storage = new Dictionary<BigInteger, long>();

        for (var j = 0; j < m; j++)
        {
            var value = BigInteger.ModPow(@base, j, mod);
            storage.TryAdd(value, j);
        }

        var baseInverse = BigInteger.ModPow(@base.ModInverse(mod), m, mod);
        for (var i = 0; i < m; i++)
        {
            var value = BigInteger.Remainder(result * BigInteger.ModPow(baseInverse, i, mod), mod);
            if (storage.TryGetValue(value, out var j))
            {
                return new BigInteger(i * m + j);
            }
        }

        throw new Exception("i and j not found");
    }
}