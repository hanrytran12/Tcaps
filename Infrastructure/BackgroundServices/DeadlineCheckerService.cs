using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices
{
    public class DeadlineCheckerService : BackgroundService
    {
        private readonly ILogger<DeadlineCheckerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DeadlineCheckerService(ILogger<DeadlineCheckerService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deadline Checker Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await CheckDeadlineAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occured while checking assignment deadlines.");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        private async Task CheckDeadlineAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Running deadline check...");

            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var today = DateOnly.FromDateTime(DateTime.Now);

                var assingmentsToUpdate = await context.Assignments.Where(a => a.EndDate == today && a.Status == "InProgress").ToListAsync(cancellationToken);
                if (assingmentsToUpdate.Any())
                {
                    foreach (var assignments in assingmentsToUpdate)
                    {
                        assignments.UpdateStatus("ReadyForTransfer");
                    }
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }

                var reworkRequestToUpdate = await context.ReworkRequests.Where(r => r.EndDate == today && (r.Status == "InProgress" || r.Status == "Approved")).ToListAsync(cancellationToken);
                if (reworkRequestToUpdate.Any())
                {
                    foreach (var reworkRequest in reworkRequestToUpdate)
                    {
                        reworkRequest.Active();
                    }
                    await unitOfWork.SaveChangesAsync();
                }

                var assignmentOutSource = await context.Assignments
                    .Where(a => a.StartDate == today
                            && a.Status == "Planned"
                            && context.Workshop.Any(w =>
                                w.Id == a.WorkshopId &&
                                w.WorkshopType == WorkshopType.Outsource))
                    .ToListAsync(cancellationToken);
                if (assignmentOutSource.Any())
                {
                    foreach (var assignment in assignmentOutSource)
                    {
                        assignment.UpdateStatus("InProgress");
                    }
                    await unitOfWork.SaveChangesAsync();
                }
            }
        }
    }
}
