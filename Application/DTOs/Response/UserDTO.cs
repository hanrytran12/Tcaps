namespace Application.DTOs.Response
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public Guid WorkshopId { get; set; }
        public string WorkshopName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
