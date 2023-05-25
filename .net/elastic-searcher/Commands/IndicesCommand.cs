using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Newtonsoft.Json;
using System;
using System.CommandLine;
using System.Text.Json;

namespace elastic_searcher.Commands
{
    internal class IndicesCommand : Command
    {
        public IndicesCommand() : base("indices", "Get indices data.")
        {
            var operation = new OperationArg();
            var indexName = new IndexNameArg();
            this.AddArgument(operation);
            this.AddArgument(indexName);
            this.SetHandler(SetHandler, operation, indexName);
        }

        private async Task SetHandler(string operation, string indexName)
        {
            switch (operation)
            {
                case "stats":
                    {
                        await OperationsHandler.HandleOperationAsync(Context.CurrentClient.Indices.StatsAsync, new IndicesStatsRequest(indices: indexName));
                        break;
                    }
                case "template":
                    {
                        await OperationsHandler.HandleOperationAsync(Context.CurrentClient.Indices.GetTemplateAsync, new GetTemplateRequest(name: indexName));
                        break;
                    }
                case "exists":
                    {
                        await OperationsHandler.HandleOperationAsync(Context.CurrentClient.Indices.ExistsAsync, new Elastic.Clients.Elasticsearch.IndexManagement.ExistsRequest(indices: indexName));
                        break;
                    }
                case "get":
                    {
                        var tmp = await Context.CurrentClient.Indices.GetAsync(indexName);
                        string jsonString = JsonConvert.SerializeObject(tmp/*.Indices*/, Formatting.Indented,);
                        ConsoleExtension.WriteSuccess(jsonString);

                        //foreach (var index in tmp.Indices)
                        //{
                        //    ConsoleExtension.WriteSuccess(index.Key.ToString());

                        //    string jsonString = JsonConvert.SerializeObject(index.Value, Formatting.Indented);
                        //    //JsonSerializer.Serialize(index.Value);

                        //    ConsoleExtension.WriteSuccess(jsonString);
                        //}


                        //await OperationsHandler.HandleOperationAsync(Context.CurrentClient.Indices.GetAsync, new GetIndexRequest(indices: indexName));
                        break;
                    }
                case "refresh":
                    {
                        await OperationsHandler.HandleOperationAsync(Context.CurrentClient.Indices.RefreshAsync, (Indices)indexName);
                        break;
                    }
                default:
                    {
                        ConsoleExtension.WriteWarning("Operation not defined");
                        break;
                    }
            }
        }
    }

    internal class OperationArg : Argument<string>
    {
        public OperationArg() : base("operation", "Name of the operation")
        { }
    }

    internal class IndexNameArg : Argument<string>
    {
        public IndexNameArg() : base("index name", "Name of the index")
        { }
    }
}
