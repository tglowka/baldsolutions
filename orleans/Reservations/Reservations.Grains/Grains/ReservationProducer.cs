using Microsoft.Extensions.Logging;
using Reservations.Interfaces.Grains;

namespace Reservations.Grains.Grains;

public sealed class ReservationProducer : LoggingGrain<ReservationProducer>, IReservationProducer, IGrainWithGuidKey
{
    public ReservationProducer(ILogger<ReservationProducer> logger) : base(logger)
    {
    }

    public Task<Guid?> AllocateRoom(Guid clientId)
    {
        Guid? roomId = Guid.NewGuid();

        _logger.LogInformation("{GrainType} {GrainKey} allocate {RoomId} room for client {ClientId}.", GrainType, GrainKey, roomId, clientId);

        return Task.FromResult(roomId);
    }
}
