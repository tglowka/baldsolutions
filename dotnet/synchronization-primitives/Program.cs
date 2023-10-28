Console.WriteLine("Choose and example:\n" +
    "1. Interlocked example\n" +
    "2. Barrier example");

var key = Console.Read();

switch (key)
{
    case '1':
        InterlockedIncrementExample.RunExample();
        break;
    case '2':
        BarrierExample.RunExample(7);
        break;
    default:
        Console.WriteLine("Unsuportted option chosen.");
        break;
}
Console.ReadKey();
