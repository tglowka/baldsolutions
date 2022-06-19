// See https://aka.ms/new-console-template for more information

public class SquareAndMultiply
{
    public static void PowerBySquareAndMultiply(int @base, int exponent)
    {
        var operations = new Stack<string>();

        while (exponent > 0)
        {
            if (exponent == 1)
            {
                operations.Push($"{@base}^{1} = {@base}");
                break;
            }
            if (exponent % 2 == 0)
            {
                var tmp = exponent;
                exponent /= 2;
                operations.Push($"{@base}^{tmp} = {@base}^{exponent} * {@base}^{exponent} - square");
            }
            else
            {
                var tmp = exponent;
                exponent -= 1;
                operations.Push($"{@base}^{tmp} = {@base}^{exponent} * {@base} - multiply");
            }
        }

        PrintOperations(operations);
    }

    private static void PrintOperations(Stack<string> operations)
    {
        var lineNumber = 1;

        while (operations.Any())
        {
            var operation = operations.Pop();
            Console.WriteLine($"{lineNumber}. {operation}");
            ++lineNumber;
        }
    }
}