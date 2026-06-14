using Microsoft.EntityFrameworkCore;
using TangaltAPI.Models;

namespace TangaltAPI.Data
{
    public class TangaltContext : DbContext
    {
        public TangaltContext(DbContextOptions<TangaltContext> options)
            : base(options) { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}