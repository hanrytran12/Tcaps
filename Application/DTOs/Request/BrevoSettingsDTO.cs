namespace Application.DTOs.Request
{
    public class BrevoSettingsDTO
    {
        public string ApiKey { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
    }
}
