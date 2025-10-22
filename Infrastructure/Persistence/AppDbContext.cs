using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Batch> Batches { get; set; }
        public DbSet<ComponentDefect> ComponentDefects { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialRequest> MaterialRequests { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Workshop> Workshop { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<Evaluate> Evaluates { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Assignment>()
                .Property(a => a.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Batch>(builder =>
            {
                builder.HasMany(o => o.Assignments)
                          .WithOne()
                          .HasForeignKey(a => a.BatchId)
                          .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
