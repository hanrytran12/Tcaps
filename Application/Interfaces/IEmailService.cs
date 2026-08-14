namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendOtpEmailAsync(string toEmail, string userName, string otpCode);
    }
}
