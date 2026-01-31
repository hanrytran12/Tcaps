using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundService
{
    public class DeadlineCheckerService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ILogger<DeadlineCheckerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly TimeSpan _period = TimeSpan.FromHours(1);

        public DeadlineCheckerService(ILogger<DeadlineCheckerService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Automatic Job Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Executing scheduled jobs...");

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var assignmentService = scope.ServiceProvider.GetRequiredService<IAssignmentAutomationService>();
                        await assignmentService.ProcessDailyUpdatesAsync(stoppingToken);

                        // 2. Lấy Rework Service (Nếu bạn tách riêng ra nữa)
                        // var reworkService = scope.ServiceProvider.GetRequiredService<IReworkAutomationService>();
                        // await reworkService.ProcessReworksAsync(stoppingToken);
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
