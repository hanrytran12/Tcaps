using Domain.Interfaces;
using Domain.Primitives;
using MediatR;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;
        public UnitOfWork(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = _context.ChangeTracker
                .Entries<AggregrateRoot>()
                .Select(e => e.Entity.DomainEvents)
                .SelectMany(e => e)
                .ToList();

            foreach (var entry in _context.ChangeTracker.Entries<AggregrateRoot>())
            {
                entry.Entity.ClearDomainEvent();
            }

            var result = await _context.SaveChangesAsync(cancellationToken);
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }
}
