using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<Batch> Batches { get; set; }
        DbSet<ComponentDefect> ComponentDefects { get; set; }
        DbSet<Income> Incomes { get; set; }
        DbSet<Inventory> Inventories { get; set; }
        DbSet<Material> Materials { get; set; }
        DbSet<MaterialRequest> MaterialRequests { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Workshop> Workshop { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
