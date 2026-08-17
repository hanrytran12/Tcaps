using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices
{
    public class DeadlineCheckerService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ILogger<DeadlineCheckerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _period;

        public DeadlineCheckerService(
            ILogger<DeadlineCheckerService> logger,
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            var intervalMinutes = configuration.GetValue<int?>("BackgroundJob:DeadlineCheckIntervalMinutes") ?? 30;
            _period = TimeSpan.FromMinutes(intervalMinutes);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Automatic Job Service started with interval: {Period}.", _period);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogDebug("Executing scheduled deadline check jobs...");

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var assignmentService = scope.ServiceProvider.GetRequiredService<IAssignmentAutomationService>();
                        await assignmentService.ProcessDailyUpdatesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing scheduled jobs.");
                }

                await Task.Delay(_period, stoppingToken);
            }
        }
    }
}
