const int threadsCount = 10_000;

Console.Write($"{nameof(StandardIncrement)}: ");
Threads(StandardIncrement); //Expected to print 10000, prints <= 10000
Console.Write($"{nameof(InterlockedIncrement)}: ");
Threads(InterlockedIncrement); //Expected to print 10000, prints 10000

static void Threads(IncrementDelegate incrementAction)
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

static void StandardIncrement(ref int arg) => ++arg;

static void InterlockedIncrement(ref int arg) => Interlocked.Increment(ref arg);

delegate void IncrementDelegate(ref int counter);

