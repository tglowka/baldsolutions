using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Reservations.Interfaces.Grains;
using Reservations.Interfaces.Strategies;

namespace Reservations.Grains.Grains;

public sealed class Manager<T> : LoggingGrain<Manager<T>>, IManager<T>
    where T : class, IGrainWithGuidKey
{
    private readonly IPersistentState<ManagerState<T>> _state;
    private readonly IEnumerable<ILoadBalancingStrategy<T>> _strategies;

    public Manager(
        [PersistentState(stateName: $"manager", storageName: "Reservations")]
        IPersistentState<ManagerState<T>> state,
        ILogger<Manager<T>> logger,
        IEnumerable<ILoadBalancingStrategy<T>> strategies
    )
        : base(logger)
    {
        _state = state;
        _strategies = strategies;
    }

    public async Task<T> GetWorker(Strategy strategy)
    {
        _state.State.GetIndex = await ChooseStrategy(strategy).GetIndex(_state.State);
        
        await _state.WriteStateAsync();

        return _state.State.Workers[_state.State.GetIndex];
    }

    public async Task AddWorker(int weight)
    {
        var worker = GrainFactory.GetGrain<T>(Guid.NewGuid());
        
        _state.State.Workers.Add(worker);
        _state.State.Weights.Add(weight);
        
        await _state.WriteStateAsync();

        _logger.LogInformation("{GrainType} {GrainKey} worker {WorkerType} count: {WorkerCount}.", GrainType, GrainKey, typeof(T).Name, _state.State.Workers.Count);
    }

    public async Task RemoveWorker()
    {
        if (_state.State.Workers.Any())
        {
            _state.State.Workers.RemoveAt(_state.State.Workers.Count - 1);
            _state.State.Weights.RemoveAt(_state.State.Weights.Count - 1);
            
            await _state.WriteStateAsync();
            
            _logger.LogInformation("{GrainType} {GrainKey} worker {WorkerType} count: {WorkerCount}.", GrainType, GrainKey, typeof(T).Name, _state.State.Workers.Count);
        }
    }

    private ILoadBalancingStrategy<T> ChooseStrategy(Strategy strategy)
        => strategy switch
        {
            Strategy.RoundRobin => _strategies.Single(x => x is IRoundRobinLoadBalancing<T>),
            Strategy.Weighted => _strategies.Single(x => x is IWeightedLoadBalancing<T>),
            _ => throw new ArgumentException(),
        };
}

[GenerateSerializer]
public record class ManagerState<T> : IManagerState<T>
{
    [Id(0)]
    public List<T> Workers { get; init; } = new();

    [Id(1)]
    public List<int> Weights { get; init; } = new();

    [Id(2)]
    public int GetIndex { get; set; } = 0;
}
