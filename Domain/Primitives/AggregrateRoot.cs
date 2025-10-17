namespace Domain.Primitives
{
    public abstract class AggregrateRoot : Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        protected void ClearDomainEvent()
        {
            _domainEvents.Clear();
        }

        protected AggregrateRoot(Guid Id) : base(Id) { }
    }
}
