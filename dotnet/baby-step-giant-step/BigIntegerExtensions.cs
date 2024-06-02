using System.Numerics;

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