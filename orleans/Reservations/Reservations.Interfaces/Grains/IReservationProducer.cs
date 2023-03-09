namespace Reservations.Interfaces.Grains;

public interface IReservationProducer : IGrainWithGuidKey
{
    public Task<Guid?> AllocateRoom(Guid clientId);
}
