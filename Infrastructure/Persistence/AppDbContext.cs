using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        public DbSet<FinalTransferRequest> FinalTransferRequests { get; set; }
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
                builder.ToTable("Batches");

                // PK map đúng cột DB
                builder.HasKey(b => b.Id);

                builder.Property(b => b.Id)
                       .HasColumnName("Id");

                // Batch - Assignment
                builder.HasMany(b => b.Assignments)
                       .WithOne(a => a.Batch)
                       .HasForeignKey(a => a.BatchId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(b => b.Product)
                       .WithMany(p => p.Batches)
                       .HasForeignKey(b => b.ProductId);
            });

            modelBuilder.Entity<Assignment>(builder =>
            {
                builder.ToTable("Assignments");
                builder.HasKey(a => a.Id);

                // Đảm bảo FK được map đúng
                builder.Property(a => a.BatchId)
                       .HasColumnName("BatchId")
                       .IsRequired(); // ✅ Thêm này để chắc chắn

                //// Relationship
                //builder.HasOne(a => a.Batch)
                //       .WithMany(b => b.Assignments)
                //       .HasForeignKey(a => a.BatchId)
                //       .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Income>()
                .Property(x => x.TotalPrice)
                .HasPrecision(18, 2);

            var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                d => d.ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified),
                d => DateOnly.FromDateTime(DateTime.SpecifyKind(d, DateTimeKind.Unspecified))
            );

            var timeOnlyConverter = new ValueConverter<TimeOnly, TimeSpan>(
                t => t.ToTimeSpan(),
                t => TimeOnly.FromTimeSpan(t)
            );

            modelBuilder.Entity<Production>(entity =>
            {
                entity.Property(p => p.Date)
                    .HasConversion(dateOnlyConverter)
                    .HasColumnType("date");

                entity.Property(p => p.Time)
                    .HasConversion(timeOnlyConverter)
                    .HasColumnType("time");
            });


            modelBuilder.Entity<AssignmentTransferRequest>()
                .Property(a => a.ReworkRequestId)
                .IsRequired(false);

            modelBuilder.Entity<Evaluate>()
                .HasMany(e => e.ComponentDefects) // Tên thuộc tính trong Evaluate Entity (ví dụ: public ICollection<ComponentDefect> ComponentDefects)
                .WithOne() // Hoặc WithOne(cd => cd.Evaluate) nếu có navigation property ngược
                .HasForeignKey(cd => cd.EvaluateId);

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

        public void ClearTracker()
        {
            base.ChangeTracker.Clear();
        }
    }
}
