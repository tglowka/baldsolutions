using Elastic.Clients.Elasticsearch;
using elastic_searcher.Commands;
using System.CommandLine;

class Program
{

    static async Task<int> Main()
    {
        while (true)
        {
            Console.Write(">>> ");
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input null or empty.");
            }

            var args = input!
                .Split(null)
                .Where(x => !string.IsNullOrEmpty(x))
                .ToArray();

            await new ElasticsearchClient().CreateAsync(
                new Tmp() { MyProperty = 7 },
                "Tmp",
                Guid.NewGuid());

            var rootCommand = new EssRootCommand();
            rootCommand.AddCommand(new PingCommand());
            rootCommand.AddCommand(new ConnectCommand());
            rootCommand.AddCommand(new ListCommand());
            rootCommand.AddCommand(new SwitchCommand());
            rootCommand.AddCommand(new ClusterCommand());
            rootCommand.AddCommand(new IndicesCommand());

            await rootCommand.InvokeAsync(args);
        }
    }
}

public class Tmp
{
    public int MyProperty { get; set; }
}