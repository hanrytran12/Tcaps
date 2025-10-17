namespace Domain.Primitives
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; private init; }

        protected Entity(Guid id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var entity = (Entity)obj;
            return Equals(entity);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(Entity? other)
        {
            if (other is null)
            {
                return false;
            }

            return other.Id == Id;
        }

        public static bool operator ==(Entity one, Entity two)
        {
            if (ReferenceEquals(one, null) && ReferenceEquals(two, null)) return true;
            if (ReferenceEquals(one, null) || ReferenceEquals(two, null)) return false;
            return one.Equals(two);
        }

        public static bool operator !=(Entity one, Entity two) => !(one == two);
    }
}
