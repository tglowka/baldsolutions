using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Transport.Products.Elasticsearch;
using elastic_searcher.Commands;
using System;
using System.Text.Json;

public class OperationsHandler
{
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

    public static async Task HandleOperationAsync<TResponse>(
            Func<CancellationToken, Task<TResponse>> operation,
            CancellationToken ct = default
        ) where TResponse : ElasticsearchResponse
    => HandleResult(await operation(ct));


    public static async Task HandleOperationAsync<TResponse, TRequest>(
            Func<TRequest, CancellationToken, Task<TResponse>> operation,
            TRequest req,
            CancellationToken ct = default
        ) where TResponse : ElasticsearchResponse
    => HandleResult(await operation(req, ct));

    private static void HandleResult<T>(T result) where T : ElasticsearchResponse
    {
        if (result.IsSuccess())
        {
            string jsonString = JsonSerializer.Serialize(result, options);

            ConsoleExtension.WriteSuccess(jsonString);
        }
        else
        {
            ConsoleExtension.WriteError($"Operation failed: {result.ElasticsearchServerError}");
        }
    }
}