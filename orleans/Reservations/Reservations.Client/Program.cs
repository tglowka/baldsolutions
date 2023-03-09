using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reservations.Client;

Console.Title = "Reservations Client";

await new HostBuilder()
    .UseOrleansClient(builder =>builder.UseLocalhostClustering())
    .ConfigureServices(
        services => services
            .AddSingleton<IHostedService, ShellHostedService>()
            .Configure<ConsoleLifetimeOptions>(sp => sp.SuppressStatusMessages = true)
    )
    .ConfigureLogging(builder => builder.AddSeq())
    .RunConsoleAsync();
