using System.Numerics;

public static class BruteForceAlgorithm
{
    public static BigInteger Compute(BigInteger @base, BigInteger result, BigInteger mod, long limit)
    {
        for (var i = 0; i < limit; i++)
        {
            var r = BigInteger.ModPow(@base, i, mod);
            if (r == result)
            {
                return i;
            }
        }
        
        throw new Exception("Not found!");
    }    
}

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

public static class BigIntegerExtensions
{
    public static BigInteger ModInverse(this BigInteger source, long mod)
        => source.ModInverse(new BigInteger(mod));

    public static BigInteger ModInverse(this BigInteger source, BigInteger mod)
    {
        BigInteger t = 0;
        BigInteger r = mod;
        BigInteger new_t = 1;
        BigInteger new_r = source;

        while (new_r != 0)
        {
            var quotient = BigInteger.Divide(r, new_r);

            (t, new_t) = (new_t, t - quotient * new_t);
            (r, new_r) = (new_r, r - quotient * new_r);
        }

        if (r > 1)
        {
            throw new Exception("a is not invertible");
        }

        if (t < 0)
        {
            t += mod;
        }

        return t;
    }
}