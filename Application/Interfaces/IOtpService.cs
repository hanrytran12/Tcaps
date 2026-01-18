namespace Application.Interfaces
{
    public interface IOtpService
    {
        Task SaveOtpAsync(string email, string otpCode);
        Task<bool> VerifyOtpAsync(string email, string otpCode);
        Task<string> CreateResetTokenAsync(string email);
        Task<string?> GetEmailByResetTokenAsync(string token);
        Task RevokeResetTokenAsync(string token);
    }
}
