using Reservations.Interfaces.Grains;
using Reservations.Interfaces.Strategies;

namespace Reservations.Grains.Strategies;

public class RoundRobinLoadBalancing<T> : IRoundRobinLoadBalancing<T>
{
    public Task<int> GetIndex(IManagerState<T> managerState)
        => Task.FromResult((managerState.GetIndex + 1) % managerState.Workers.Count);
}
