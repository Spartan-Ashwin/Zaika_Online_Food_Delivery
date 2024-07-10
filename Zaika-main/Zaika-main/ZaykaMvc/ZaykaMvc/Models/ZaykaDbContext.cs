using Microsoft.EntityFrameworkCore;

namespace ZaykaMvc.Models
{
    public class ZaykaDbContext : DbContext
    {
        public ZaykaDbContext(DbContextOptions<ZaykaDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Dish>().HasKey(d => d.Id);
            modelBuilder.Entity<Order>().HasKey(o => o.Id);
        }
    }
}
