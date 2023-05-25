using Elastic.Clients.Elasticsearch;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace elastic_searcher.Commands
{
    internal class ConnectCommand : Command
    {
        public ConnectCommand() : base(ConnectMessages.Name, ConnectMessages.Description)
        {
            var arg = new UriArgument();
            this.AddArgument(arg);
            this.SetHandler(SetHandler, arg);
        }

        private async Task SetHandler(Uri? uri)
        {
            var client = CreateClient(uri);
            var testResponse = await client.PingAsync();

            var key = uri!.OriginalString;

            if (testResponse.IsSuccess())
            {
                if (Context.ClientExists(key))
                {
                    ConsoleExtension.WriteWarning(ConnectMessages.ConnectionExists);
                }
                else
                {
                    Context.AddClient(key, client);
                    Context.SetCurrentURI(key);
                    ConsoleExtension.WriteSuccess(ConnectMessages.ConnectionAdded);
                }
            }
            else
            {
                ConsoleExtension.WriteError(ConnectMessages.ConnectionFailed);
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

    internal class UriArgument : Argument<Uri?>
    {
        public UriArgument() : base(
            name: ConnectMessages.ArgName,
            parse: Parse,
            isDefault: true,
            description: ConnectMessages.ArgDescription)
        { }

        private static Uri? Parse(ArgumentResult result)
        {
            switch (result.Tokens)
            {
                case []:
                    {
                        return new Uri("http://localhost:9200");
                    }
                case [var uriString]:
                    {
                        if (string.IsNullOrEmpty(uriString.Value))
                        {
                            result.ErrorMessage = ConnectMessages.ArgEmpty;
                            return null;
                        }

                        var created = Uri.TryCreate(uriString.Value, UriKind.Absolute, out var uri);

                        if (!created)
                        {
                            result.ErrorMessage = ConnectMessages.ArgNotWellFormatted;
                        }

                        return uri;
                    }
                default:
                    {
                        result.ErrorMessage = ConnectMessages.TooManyArgs;
                        return null;
                    }
            }
        }
    }

    internal class ConnectMessages
    {
        internal const string Name = "connect";
        internal const string Description = "Setup a connection to the Elasticsearch.";
        internal const string ConnectionExists = "Connection exists in the connection pool.";
        internal const string ConnectionAdded = "Connection added to the connection pool.";
        internal const string ConnectionFailed = "Connection failed.";
        internal const string ArgName = "URI";
        internal const string ArgDescription = "Address of the Elasticsearch.";
        internal const string ArgEmpty = "URI cannot be empty.";
        internal const string ArgNotWellFormatted = "URI is not well formed.";
        internal const string TooManyArgs = "Too many arguments.";
    }
}
