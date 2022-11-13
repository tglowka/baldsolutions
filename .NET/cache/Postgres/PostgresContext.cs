using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace cache.Postgres
{
    public class PostgresContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public PostgresContext(DbContextOptions<PostgresContext> options) : base(options)
        {
        }
    }

    public class Post
    {
        [Key]
        public Guid Id { get; set; }
        public string Content { get; set; }

        public Post(Guid id, string content)
        {
            Id = id;
            Content = content;
        }
    }
}
