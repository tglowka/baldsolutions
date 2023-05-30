// See https://aka.ms/new-console-template for more information

using System.Numerics;

public class Program
{
    public static void Main() { }
}

public static class VectorizationExtension
{
    public static int Max_Vectorized(this IEnumerable<int> source)
    {
        if (source.GetType() == typeof(int[]))
        {
            var array = (int[])source;

            if (Vector.IsHardwareAccelerated && array.Length >= Vector<int>.Count * 2)
            {
                var maxes = new Vector<int>(array);
                var index = Vector<int>.Count;

                var threshold = array.Length - Vector<int>.Count;

                do
                {
                    maxes = Vector.Max(maxes, new Vector<int>(array, index));
                    index += Vector<int>.Count;
                } while (index <= threshold);

                maxes = Vector.Max(maxes, new Vector<int>(array, threshold));

                var value = maxes[0];

                for (int i = 1; i < Vector<int>.Count; i++)
                {
                    if (maxes[i] > value)
                    {
                        value = maxes[i];
                    }
                }

                return value;
            }
            else
            {
                var max = array[0];

                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] > max)
                    {
                        max = array[i];
                    }
                }

                return max;
            }
        }

        throw new ArgumentException();
    }
}

