using Microsoft.Extensions.Hosting;
using Reservations.Interfaces.Grains;

namespace Reservations.Client;

public sealed class ShellHostedService : BackgroundService
{
    private readonly IClusterClient _client;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly Random _random;

    public ShellHostedService(IClusterClient client, IHostApplicationLifetime applicationLifetime)
    {
        _client = client;
        _applicationLifetime = applicationLifetime;
        _random = new Random();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await PrepareEnvironment();
        await Run(stoppingToken);
    }

    private async Task PrepareEnvironment()
    {
        await _client.GetGrain<IManager<IBroker>>(Managers.BrokerManager).AddWorker(0);
        await _client.GetGrain<IManager<IReservationProducer>>(Managers.ReservationProducerManager).AddWorker(100);
    }

    private async Task Run(CancellationToken stoppingToken)
    {
        var cts = new List<CancellationTokenSource>();

        while (!stoppingToken.IsCancellationRequested)
        {
            Console.Clear();
            ShowHelp();

            var command = Console.ReadLine();

            if (command is "1")
            {
                CreateReservation(cts);
            }
            else if (command is "2")
            {
                StopReservationCreation(cts);
            }
            else if (command is "3")
            {
                await AddBroker();
            }
            else if (command is "4")
            {
                await AddReservationProducer();
            }
            else if (command is "5")
            {
                await RemoveBroker();
            }
            else if (command is "6")
            {
                await RemoveReservationProducer();
            }
            else if (command is "7")
            {
                _applicationLifetime.StopApplication();
            }
        }
    }

    private void CreateReservation(List<CancellationTokenSource> ctsList)
    {
        var cts = new CancellationTokenSource();

        var task = Task.Run(async () =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                var delay = _random.Next(200,300);

                var client = _client.GetGrain<IClient>(Guid.NewGuid());
                await client.CreateReservation();

                await Task.Delay(delay);
            }
        });

        ctsList.Add(cts);
    }

    private void StopReservationCreation(List<CancellationTokenSource> cts)
    {
        foreach (var c in cts)
        {
            c.Cancel();
        }

        cts.Clear();
    }

    private async Task AddBroker()
    {
        await _client.GetGrain<IManager<IBroker>>(Managers.BrokerManager).AddWorker(0);
    }

    private async Task AddReservationProducer()
    {
        Console.Write("Comma separated weights:");

        var weights = Console.ReadLine();

        if (string.IsNullOrEmpty(weights))
        {
            Console.WriteLine("Invalid weights!");
            return;
        }

        foreach (var weight in weights.Split(',').Select(int.Parse))
        {
            await _client.GetGrain<IManager<IReservationProducer>>(Managers.ReservationProducerManager).AddWorker(weight);

        };

    }

    private async Task RemoveBroker()
    {
        await _client.GetGrain<IManager<IBroker>>(Managers.BrokerManager).RemoveWorker();
    }

    private async Task RemoveReservationProducer()
    {
        await _client.GetGrain<IManager<IReservationProducer>>(Managers.ReservationProducerManager).RemoveWorker();
    }

    private static void ShowHelp()
    {
        Console.WriteLine(
            "1. Create reservation\n" +
            "2. Stop creations\n" +
            "3. Add broker\n" +
            "4. Add reservation producer\n" +
            "5. Remove broker\n" +
            "6. Remove reservation producer\n" +
            "7. Quit"
            );
    }
}
