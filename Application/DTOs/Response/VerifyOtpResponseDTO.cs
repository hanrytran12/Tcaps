namespace Application.DTOs.Response
{
    public class VerifyOtpResponseDTO
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

        public VerifyOtpResponseDTO() { }

        public VerifyOtpResponseDTO(string message, string token)
        {
            Message = message;
            Token = token;
        }
    }
}
