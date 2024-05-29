using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<SuperHero> SuperHeroes { get; set; }
    }
}
