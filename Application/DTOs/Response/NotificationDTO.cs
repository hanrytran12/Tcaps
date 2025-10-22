namespace Application.DTOs.Response
{
    public class NotificationDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateOnly CreatedDate { get; set; }
    }
}
