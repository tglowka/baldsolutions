public class BranchMispredictionExecutor
{
    private readonly List<int> _sortedCollection;
    private readonly List<int> _unsortedCollection;

    public BranchMispredictionExecutor(int count)
    {
        _sortedCollection = Enumerable.Range(0, count).ToList();
        _unsortedCollection = Enumerable.Range(0, count).ToList();
        _unsortedCollection.Shuffle();
    }

    public int SortedSumLessThan(int threshold)
    {
        var sum = 0;

        for (int i = 0; i < _sortedCollection.Count; i++)
        {
            var element = _sortedCollection[i];
            if (element < threshold)
                sum += element;
        }

        return sum;
    }

    public int UnsortedSumLessThan(int threshold)
    {
        var sum = 0;

        for (int i = 0; i < _unsortedCollection.Count; i++)
        {
            var element = _unsortedCollection[i];
            if (element < threshold)
                sum += element;
        }

        return sum;
    }
}

public static class BranchMispredictionExecutorExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        var random = new Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}