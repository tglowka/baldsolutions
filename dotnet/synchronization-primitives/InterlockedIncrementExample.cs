
public class InterlockedIncrementExample
{
    const int threadsCount = 10_000;

    public static void RunExample()
    {
        Console.Write($"{nameof(StandardIncrement)}: ");
        Threads(StandardIncrement); //Expected to print 10000, prints <= 10000
        Console.Write($"{nameof(InterlockedIncrement)}: ");
        Threads(InterlockedIncrement); //Expected to print 10000, prints 10000
    }

    public static void Threads(IncrementDelegate incrementAction)
    {
        var counter = 0;
        Thread[] threads = new Thread[threadsCount];

        for (int i = 0; i < threadsCount; i++)
        {
            var thread = new Thread(() =>
            {
                Thread.Sleep(100);
                incrementAction(ref counter);
            });
            threads[i] = thread;
            thread.Start();
        }

        for (int i = 0; i < threadsCount; i++)
        {
            threads[i].Join();
        }

        Console.WriteLine(counter);
    }

    public static void StandardIncrement(ref int arg) => ++arg;

    public static void InterlockedIncrement(ref int arg) => Interlocked.Increment(ref arg);

    public delegate void IncrementDelegate(ref int counter);
}