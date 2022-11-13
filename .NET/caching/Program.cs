using Newtonsoft.Json;
using System.Text;

public class Program
{
    static async Task Main(string[] args)
    {
    }
}

public class Client
{
    private static readonly HttpClient _client = new();
    private static readonly string _url = "http://localhost:5000/api/posts";

    public static async Task<string> CreatePost()
    {
        var response = await _client.PostAsync($"{_url}", CreateHttpContent());
        return GetId(response);
    }


    public static async Task<string> GetPost(string id, bool useInMemoryCache, bool useRedisCache)
    {
        var response = await _client.GetAsync($"{_url}?id={id}&useInMemoryCache={useInMemoryCache}&useRedisCache={useRedisCache}");

        return await response.Content.ReadAsStringAsync();
    }

    private static PostBody CreatePostBody()
        => new PostBody(Guid.NewGuid().ToString());

    private static HttpContent CreateHttpContent()
        => new StringContent(JsonConvert.SerializeObject(CreatePostBody()), Encoding.UTF8, "application/json");

    private static string GetId(HttpResponseMessage response)
    {
        response.Headers.TryGetValues("Id", out var id);
        return id.First();
    }
}

public class PostBody
{
    public string Content { get; set; }

    public PostBody(string content)
    {
        Content = content;
    }
}