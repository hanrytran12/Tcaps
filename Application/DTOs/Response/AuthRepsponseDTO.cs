namespace Application.DTOs.Response
{
    public class AuthRepsponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
