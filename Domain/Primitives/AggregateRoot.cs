using MediatR;

namespace Domain.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<INotification> _domainEvents = new();

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected AggregateRoot(Guid Id) : base(Id) { }
    }
}
