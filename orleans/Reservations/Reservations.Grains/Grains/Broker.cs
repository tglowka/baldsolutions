using Microsoft.Extensions.Logging;
using Reservations.Interfaces.Grains;
using Reservations.Interfaces.Strategies;

namespace Reservations.Grains.Grains;

public sealed class Broker : LoggingGrain<Broker>, IBroker
{
    public Broker(ILogger<Broker> logger) : base(logger)
    {
    }

    public async Task<Guid?> CreateReservation(Guid clientId)
    {
        var reservationProducer = await GrainFactory
            .GetGrain<IManager<IReservationProducer>>(Managers.ReservationProducerManager)
            .GetWorker(Strategy.Weighted);

        _logger.LogInformation("{GrainType} {GrainKey} reservation creation process in progress.", GrainType, GrainKey);
        var roomId = await reservationProducer.AllocateRoom(clientId);

        return roomId;
    }
}
