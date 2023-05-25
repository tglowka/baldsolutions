using Elastic.Clients.Elasticsearch;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace elastic_searcher.Commands
{
    internal class PingCommand : Command
    {
        public PingCommand() : base(PingMessages.Name, PingMessages.Description)
        {
            var arg = new UriArgument();
            this.AddArgument(arg);
            this.SetHandler(SetHandler, arg);
        }

        private async Task SetHandler(Uri? uri)
        {
            var client = CreateClient(uri);
            var testResponse = await client.PingAsync();

            if (testResponse.IsSuccess())
            {
                ConsoleExtension.WriteSuccess(PingMessages.PingSucceeded);
            }
            else
            {
                ConsoleExtension.WriteError(PingMessages.PingFailed);
            }
        }

        private static ElasticsearchClient CreateClient(Uri? uri)
        {
            if (uri is null)
            {
                throw new ArgumentException(nameof(CreateClient), nameof(uri));
            }

            var settings = new ElasticsearchClientSettings(uri).PrettyJson();
            return new ElasticsearchClient(settings);
        }
    }
}

internal class PingMessages
{
    internal const string Name = "ping";
    internal const string Description = "Test the reachability of the Elasticsearch node.";
    internal const string PingSucceeded = "Ping succeeded.";
    internal const string PingFailed = "Ping failed.";
}