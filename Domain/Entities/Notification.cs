using Domain.Primitives;

namespace Domain.Entities
{
    public class Notification : Entity
    {
        public Guid UserId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Message { get; private set; } = string.Empty;
        public string Type { get; private set; } = string.Empty;
        public bool IsRead { get; private set; }
        public DateOnly CreatedAt { get; private set; }

        public Notification(Guid id, Guid userId, string title, string message, string type)
            : base(id)
        {
            UserId = userId;
            Title = title;
            Message = message;
            Type = type;
            CreatedAt = DateOnly.FromDateTime(DateTime.Now);
        }

        private Notification() : base(Guid.NewGuid()) { }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
