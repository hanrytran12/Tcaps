namespace Application.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendOtplEmailAsync(string toEmail, string userName, string otpCode);
    }
}
