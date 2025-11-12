using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public AppDbContext(DbContextOptions options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
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
        public DbSet<MaterialUse> MaterialUse { get; set; }
        public DbSet<AssignmentTransferRequest> AssignmentTransferRequests { get; set; }
        public DbSet<MaterialWorkshop> MaterialWorkshops { get; set; }
        public DbSet<TaskTransferRequest> TaskTransferRequests { get; set; }
        public DbSet<WorkshopInventory> WorkshopInventory { get; set; }
        public DbSet<ReworkRequest> ReworkRequests { get; set; }
        public DbSet<MaterialSupply> MaterialSupplies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Assignment>()
                .Property(a => a.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<MaterialUse>()
                .Property(mu => mu.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Batch>(builder =>
            {
                builder.HasMany(o => o.Assignments)
                          .WithOne()
                          .HasForeignKey(a => a.BatchId)
                          .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Income>()
                .Property(x => x.TotalPrice)
                .HasPrecision(18, 2);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                var domainEvents = ChangeTracker
                .Entries<AggregrateRoot>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .SelectMany(e =>
                {
                    var events = e.DomainEvents.ToList();
                    e.ClearDomainEvent();
                    return events;
                }).ToList();

                if (!domainEvents.Any())
                {
                    break;
                }

                foreach (var domainEvent in domainEvents)
                {
                    await _mediator.Publish(domainEvent, cancellationToken);
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
