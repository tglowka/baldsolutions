using System.Net.Http;
using System.Threading.Tasks;

namespace async_eliding_part_2
{
    public class HttpClientExecutor
    {
        private readonly string _uri = "https://postman-echo.com/get?foo1=bar1&foo2=bar2";

        public async Task<string> GetWithUsingAndWithoutEliding()
        {
            using var _httpClient = new HttpClient();
            return await _httpClient.GetStringAsync(_uri);
        }

        public Task<string> GetWithUsingAndWithEliding()
        {
            using var _httpClient = new HttpClient();
            return _httpClient.GetStringAsync(_uri);
        }
    }
}