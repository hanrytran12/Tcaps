using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Batch> Batches { get; }
        public DbSet<ComponentDefect> ComponentDefects { get; }
        public DbSet<Income> Incomes { get; }
        public DbSet<Inventory> Inventories { get; }
        public DbSet<Material> Materials { get; }
        public DbSet<MaterialRequest> MaterialRequests { get; }
        public DbSet<MaterialUse> MaterialUse { get; }
        public DbSet<Notification> Notifications { get; }
        public DbSet<Product> Products { get; }
        public DbSet<User> Users { get; }
        public DbSet<Workshop> Workshop { get; }
        public DbSet<Assignment> Assignments { get; }
        public DbSet<Production> Productions { get; }
        public DbSet<Evaluate> Evaluates { get; }
        public DbSet<AssignmentTransferRequest> AssignmentTransferRequests { get; }
        public DbSet<TaskTransferRequest> TaskTransferRequests { get; }
    }
}
