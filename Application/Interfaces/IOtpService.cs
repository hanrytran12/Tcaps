namespace Application.Interfaces
{
    public interface IOtpService
    {
        Task SaveOtpAsync(string email, string otpCode);
        Task<bool> VerifyOtpAsync(string email, string otpCode);
    }
}
