using Reservations.Interfaces.Strategies;

namespace Reservations.Interfaces.Grains;

public static class Managers
{
    public static readonly Guid BrokerManager = Guid.Parse("33f35194-339a-48bf-aee5-22d258dd0bf4");
    public static readonly Guid ReservationProducerManager = Guid.Parse("43f35194-339a-48bf-aee5-22d258dd0bf4");
}

public interface IManager<T> : IGrainWithGuidKey
    where T : IGrainWithGuidKey
{
    public Task<T> GetWorker(Strategy strategy);
    public Task AddWorker(int weight);
    public Task RemoveWorker();
}

public interface IManagerState<T>
{
    public List<T> Workers { get; init; }

    public List<int> Weights { get; init; }

    public int GetIndex { get; set; }
}
