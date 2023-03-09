namespace Reservations.Interfaces.Grains;

public interface IBroker : IGrainWithGuidKey
{
    public Task<Guid?> CreateReservation(Guid clientId);
}
