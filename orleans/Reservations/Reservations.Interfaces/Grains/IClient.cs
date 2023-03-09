namespace Reservations.Interfaces.Grains;

public interface IClient : IGrainWithGuidKey
{
    Task CreateReservation();
}
