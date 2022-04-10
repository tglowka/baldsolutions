// See https://aka.ms/new-console-template for more information
public class FalseSharingExecutor
{
    private readonly int _processorCount = Environment.ProcessorCount;
    private readonly int _iterations;

    public readonly int[] _list, _list2, _list3;

    public FalseSharingExecutor(int iterations)
    {
        _list = new int[_processorCount];
        _list2 = new int[_processorCount * 16];
        _list3 = new int[_processorCount * 16 + 16];
        _iterations = iterations;
    }

    public async Task ExecuteWithFalseSharing()
    {
        var tasks = new Task[_processorCount];

        for (int i = 0; i < _processorCount; i++)
        {
            var index = i;

            tasks[index] = Task.Run(() =>
            {
                for (int j = 0; j < _iterations; j++)
                    ++_list[index];
            });
        }

        await Task.WhenAll(tasks);
    }

    public async Task ExecuteFalseSharingWithImprovement()
    {
        var tasks = new Task[_processorCount];

        for (int i = 0; i < _processorCount; i++)
        {
            var index = i;

            tasks[index] = Task.Run(() =>
            {
                for (int j = 0; j < _iterations; j++)
                    ++_list2[index * 16];
            });
        }

        await Task.WhenAll(tasks);
    }

    public async Task ExecuteFalseSharingWithBetterImprovement()
    {
        var tasks = new Task[_processorCount];

        for (int i = 0; i < _processorCount; i++)
        {
            var index = i;

            tasks[i] = Task.Run(() =>
            {
                for (int j = 0; j < _iterations; j++)
                    ++_list3[index * 16 + 16];
            });
        }

        await Task.WhenAll(tasks);
    }
}