public class Closure
{
    public static Func<int> GetFuncWithClosure(int arg)
    {
        int internalVariable = arg;

        var func = new Func<int>(() =>
        {
            internalVariable -= 5;

            return 10 / internalVariable;
        });

        return func;
    }

    public static Func<int, int> GetFuncWithoutClosure()
    {
        var func = new Func<int, int>((arg) =>
        {
            arg -= 5;

            return 10 / arg;
        });

        return func;
    }
}
