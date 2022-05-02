// See https://aka.ms/new-console-template for more information
public class StackallocExecutor
{
    private readonly int _size;
    private readonly Random _random;

    public StackallocExecutor(int size)
    {
        _size = size;
        _random = new Random();
    }

    public int Stackalloc()
    {
        Span<int> span = stackalloc int[_size];

        for (int i = 0; i < span.Length; i++)
            span[i] = _random.Next();

        for (int i = 0; i < span.Length; i++)
            if (i % 2 == 1)
                span[i] += 1;

        var max = span[0];

        for (int i = 1; i < span.Length; i++)
            if (span[i] > max)
                max = span[i];

        return max;
    }

    public int NoStackalloc()
    {
        var array = new int[_size];

        for (int i = 0; i < array.Length; i++)
            array[i] = _random.Next();

        for (int i = 0; i < array.Length; i++)
            if (i % 2 == 1)
                array[i] += 1;

        var max = array[0];

        for (int i = 1; i < array.Length; i++)
            if (array[i] > max)
                max = array[i];

        return max;
    }
}