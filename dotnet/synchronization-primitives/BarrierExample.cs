using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

public static class BarrierExample
{
    private static Barrier _barrier;
    private static ConcurrentDictionary<Guid, string> _phase0Hashes = new();
    private static ConcurrentDictionary<Guid, string> _phase1Hashes = new();

    public static void RunExample(int participantCount)
    {
        SetBarrier(participantCount);
        StartPhases(participantCount);
    }

    public static void SetBarrier(int participantCount)
        => _barrier = new Barrier(
                participantCount,
                (b) =>
                {
                    switch (b.CurrentPhaseNumber)
                    {
                        case 0: PostPhase0Action(); break;
                        case 1: PostPhase1Action(); break;
                        default: break;
                    }

                    void PostPhase0Action()
                    {
                        Console.WriteLine($"\nPhase: {b.CurrentPhaseNumber} finished - hashes found: ");
                        foreach (var (guid, hash) in _phase0Hashes)
                            Console.WriteLine($"Guid: {guid}, Hash: {hash}");
                    }

                    void PostPhase1Action()
                    {
                        Console.WriteLine($"\nPhase: {b.CurrentPhaseNumber} finished - more hashes found.");
                        foreach (var (guid, hash) in _phase1Hashes)
                            Console.WriteLine($"Guid: {guid}, Hash: {hash}");
                    }
                }
                );

    public static void StartPhases(int participantCount)
    {
        Console.WriteLine("Phases started!");
        var threads = new Thread[participantCount];
        for (int i = 0; i < participantCount; i++)
            threads[i] = new Thread(() =>
            {
                Phase0();
                Phase1();
            });
        for (int i = 0; i < participantCount; i++)
            threads[i].Start();

        void Phase0()
        {
            FindAndStoreHash(hashesStorage: _phase0Hashes, leadingZeroCount: 2);
            _barrier.SignalAndWait();
        }

        void Phase1()
        {
            FindAndStoreHash(hashesStorage: _phase1Hashes, leadingZeroCount: 3);
            _barrier.SignalAndWait();
        }
    }

    public static void FindAndStoreHash(ConcurrentDictionary<Guid, string> hashesStorage, int leadingZeroCount)
    {
        Guid guid;
        byte[] hash;

        while (true)
        {
            guid = Guid.NewGuid();
            hash = SHA256.HashData(guid.ToByteArray());
            if (hash.Take(leadingZeroCount).All(x => x == 0))
                break;
        }

        if (!hashesStorage.TryAdd(guid, HashToString(hash)))
            throw new Exception($"Guid collision: {guid}");
    }

    public static string HashToString(byte[] hash)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < hash.Length; i++)
            sb.Append(hash[i].ToString("x2"));

        return sb.ToString();
    }
}