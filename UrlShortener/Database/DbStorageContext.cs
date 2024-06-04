using Microsoft.EntityFrameworkCore;
using UrlShortener.Entities;

namespace UrlShortener.Database
{
    public class DbStorageContext : DbContext
    {
        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
        public DbSet<User> Users { get; set; }

        public DbStorageContext(DbContextOptions<DbStorageContext> options) : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }
    }
}