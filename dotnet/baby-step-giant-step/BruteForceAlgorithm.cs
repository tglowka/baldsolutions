using System.Numerics;

namespace baby_step_giant_step;

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