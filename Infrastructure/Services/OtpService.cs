using Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly IDistributedCache _distributedCache;

        public OtpService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task SaveOtpAsync(string email, string otpCode)
        {
            var key = $"reset_pass:{email}";

            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            await _distributedCache.SetStringAsync(key, otpCode, options);
        }

        public async Task<bool> VerifyOtpAsync(string email, string otpCode)
        {
            var key = $"reset_pass:{email}";

            var cacheOtp = await _distributedCache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cacheOtp))
            {
                return false;
            }

            if (cacheOtp == otpCode)
            {
                await _distributedCache.RemoveAsync(key);
                return true;
            }

            return false;
        }

        public async Task<string> CreateResetTokenAsync(string email)
        {
            string token = Guid.NewGuid().ToString();

            await _distributedCache.SetStringAsync($"reset_token:{token}", email, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            });

            return token;
        }

        public async Task<string?> GetEmailByResetTokenAsync(string token)
        {
            var key = $"reset_token:{token}";
            return await _distributedCache.GetStringAsync(key);
        }

        public async Task RevokeResetTokenAsync(string token)
        {
            var key = $"reset_token:{token}";
            await _distributedCache.RemoveAsync(key);
        }
    }
}
