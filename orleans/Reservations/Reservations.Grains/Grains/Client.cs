using Microsoft.Extensions.Logging;
using Reservations.Interfaces.Grains;
using Reservations.Interfaces.Strategies;

namespace Reservations.Grains.Grains;

public class Client : LoggingGrain<Client>, IClient
{
    public Client(ILogger<Client> logger) : base(logger)
    {
    }

    public async Task CreateReservation()
    {
        _logger.LogInformation("{GrainType} {GrainKey} reservation creation process started.", GrainType, GrainKey);
        
        var broker = await GrainFactory
            .GetGrain<IManager<IBroker>>(Managers.BrokerManager)
            .GetWorker(Strategy.RoundRobin);

        await broker.CreateReservation(GrainKey);

        _logger.LogInformation("{GrainType} {GrainKey} reservation creation process finished.", GrainType, GrainKey);
    }
}
