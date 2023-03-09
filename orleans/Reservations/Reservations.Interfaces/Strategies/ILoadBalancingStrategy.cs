using Reservations.Interfaces.Grains;

namespace Reservations.Interfaces.Strategies;

public enum Strategy
{
    RoundRobin,
    Weighted
}

public interface ILoadBalancingStrategy<T>
{
    Task<int> GetIndex(IManagerState<T> manager);
}
