// See https://aka.ms/new-console-template for more information
using System.Buffers;

public class ArrayExecutor
{
    private readonly int _size;
    private readonly Random _random;

    public ArrayExecutor(int size)
    {
        _random = new Random();
        _size = size;
    }

    public void ArrayLoop()
    {
        var array = new int[_size];

        for (int i = 0; i < _size; i++)
            array[i] = _random.Next(100);

        var minMax = new int[2]
        {
            array.Min(), array.Max()
        };

        Process(minMax);
    }

    public void ArrayPoolLoop()
    {
        var array = ArrayPool<int>.Shared.Rent(_size);

        for (int i = 0; i < _size; i++)
            array[i] = _random.Next(100);

        var minMax = new int[2]
        {
            array.Min(), array.Max()
        };

        Process(minMax);

        ArrayPool<int>.Shared.Return(array);
    }

    private void Process(int[] array)
    {
        // some computations here
    }
}