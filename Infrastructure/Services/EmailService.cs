using Application.DTOs.Request;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly BrevoSettingsDTO _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(HttpClient httpClient, IOptions<BrevoSettingsDTO> settings, ILogger<EmailService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;

            _httpClient.BaseAddress = new Uri("https://api.brevo.com/v3/");
            _httpClient.DefaultRequestHeaders.Add("api-key", _settings.ApiKey);
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
        }

        public async Task<bool> SendOtpEmailAsync(string toEmail, string userName, string otpCode)
        {
            var payload = new
            {
                sender = new
                {
                    name = _settings.SenderName,
                    email = _settings.SenderEmail
                },
                to = new[]
                {
                    new { email = toEmail, name = userName }
                },
                templateId = 2,
                @params = new
                {
                    otp_code = otpCode
                }
            };

            var jsonContent = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("smtp/email", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("Send email failed: {ErrorBody}", errorBody);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending email to {Email}", toEmail);
                return false;
            }
        }
    }
}