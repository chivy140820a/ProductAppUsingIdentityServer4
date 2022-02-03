using Microsoft.EntityFrameworkCore;
using ProductApp.API.Configurations;
using ProductApp.API.Entity;

namespace ProductApp.API
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> context) : base(context)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }

    }
}
