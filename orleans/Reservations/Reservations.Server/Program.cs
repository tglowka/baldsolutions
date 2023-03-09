using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reservations.Grains.Strategies;
using Reservations.Interfaces.Strategies;

Console.Title = "Reservations Server";

await Host.CreateDefaultBuilder(args)
    .UseOrleans(
        builder => builder
            .UseLocalhostClustering()
            .AddMemoryGrainStorage("Reservations")
            .UseDashboard()
            .ConfigureServices(x => x
                .AddScoped(typeof(ILoadBalancingStrategy<>), typeof(WeightedLoadBalancing<>))
                .AddScoped(typeof(ILoadBalancingStrategy<>), typeof(RoundRobinLoadBalancing<>))
            )
    )
    .ConfigureLogging( builder => builder.AddSeq())
    .RunConsoleAsync();
