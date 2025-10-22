using MediatR;

namespace Domain.Primitives
{
    public abstract class AggregrateRoot : Entity
    {
        private readonly List<INotification> _domainEvents = new();

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvent()
        {
            _domainEvents.Clear();
        }

        protected AggregrateRoot(Guid Id) : base(Id) { }
    }
}
