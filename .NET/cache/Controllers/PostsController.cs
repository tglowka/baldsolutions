using cache.Postgres;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace cache.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _redisCache;
        private readonly PostgresContext _postgresContext;

        public PostsController(
            IMemoryCache memoryCache,
            IDistributedCache redisCache,
            PostgresContext postgresContext)
        {
            _memoryCache = memoryCache;
            _redisCache = redisCache;
            _postgresContext = postgresContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Post post)
        {
            var model = new Postgres.Post(
                Guid.NewGuid(),
                post.Content
            );

            SaveInMemoryCache(model);
            await SaveInRedisCache(model);
            await SaveInPostgres(model);

            HttpContext.Response.Headers.Add("Id", model.Id.ToString());

            return Ok(model.Id);
        }

        [HttpGet]
        public async Task<string> Get(
            [FromQuery] string id,
            [FromQuery] bool useInMemoryCache = false,
            [FromQuery] bool useRedisCache = false)
        {
            if (useInMemoryCache)
                return _memoryCache.Get<string>(id);

            if (useRedisCache)
                return await _redisCache.GetStringAsync(id);

            return _postgresContext
                .Set<Postgres.Post>()
                .Single(x => x.Id == Guid.Parse(id))
                .Content;
        }

        private void SaveInMemoryCache(Postgres.Post model)
            => _memoryCache.Set(model.Id.ToString(), model.Content);

        private async Task SaveInRedisCache(Postgres.Post model)
            => await _redisCache.SetStringAsync(model.Id.ToString(), model.Content);

        private async Task SaveInPostgres(Postgres.Post model)
        {
            await _postgresContext.Set<Postgres.Post>().AddAsync(model);
            await _postgresContext.SaveChangesAsync();
        }
    }
}

public class Post
{
    public string Content { get; set; }

    public Post(string content)
    {
        Content = content;
    }
}
