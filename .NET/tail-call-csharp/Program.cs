Recursion.StandardRecursion(6000000);
Recursion.TailRecursion(6000000);

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
