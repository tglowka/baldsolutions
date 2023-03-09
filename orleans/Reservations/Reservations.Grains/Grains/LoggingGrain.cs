using Microsoft.Extensions.Logging;

namespace Reservations.Grains.Grains;

public abstract class LoggingGrain<T> : Grain
{
    protected Guid GrainKey => this.GetPrimaryKey();
    protected string GrainType => typeof(T).Name;

    protected readonly ILogger<T> _logger;

    public LoggingGrain(ILogger<T> logger)
    {
        _logger = logger;
    }
}
