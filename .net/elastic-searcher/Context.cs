using Elastic.Clients.Elasticsearch;

public class Context
{
    private readonly static Dictionary<string, ElasticsearchClient> _clients = new();

    public static KeyValuePair<string, ElasticsearchClient>[] Clients
        => _clients
            .OrderBy(x => x.Key)
            .ToArray();

    public static void AddClient(string key, ElasticsearchClient value)
        => _clients.Add(key, value);

    public static bool ClientExists(string key)
        => _clients.ContainsKey(key);

    public static void SetCurrentURI(byte ordinal)
        => SetCurrentURI(Clients[ordinal - 1].Key);

    public static void SetCurrentURI(string uri)
        => CurrentURI = uri;

    public static string CurrentURI = string.Empty;

    public static ElasticsearchClient CurrentClient
        => _clients[CurrentURI];

    public static bool HasAnyClients => Clients.Any();

    public static bool HasZeroClients => !HasAnyClients;
}