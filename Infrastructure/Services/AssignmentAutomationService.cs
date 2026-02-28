using API.Hubs;
using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class AssignmentAutomationService : IAssignmentAutomationService
    {
        private readonly IAppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _hubContext; // SignalR
        private readonly ILogger<AssignmentAutomationService> _logger;

        public AssignmentAutomationService(
            IAppDbContext context,
            IUnitOfWork unitOfWork,
            IHubContext<NotificationHub> hubContext,
            ILogger<AssignmentAutomationService> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task ProcessDailyUpdatesAsync(CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            await CheckDueAssignmentsAsync(today, cancellationToken);

            await CheckOutsourceStartAsync(today, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task CheckDueAssignmentsAsync(DateOnly today, CancellationToken token)
        {
            var assignments = await _context.Assignments
                .Include(a => a.Batch)
                .Where(a => a.EndDate == today && a.Status == "InProgress")
                .ToListAsync(token);

            var QCId = await _context.Users.AsNoTracking()
                .Where(u => u.Role == "QC")
                .Select(u => u.Id)
                .FirstOrDefaultAsync(token);

            if (!assignments.Any()) return;

            foreach (var assignment in assignments)
            {
                assignment.UpdateStatus("ReadyForTransfer");

                await _hubContext.Clients.User(QCId.ToString())
                    .SendAsync("AssignmentStatusChanged", new
                    {
                        Id = assignment.Id,
                        Status = "ReadyForTransfer",
                        Message = $"Lô {assignment.Batch.Code} đã đến hạn deadline."
                    }, token);
            }
            _logger.LogInformation($"Updated {assignments.Count} assignments to ReadyForTransfer.");
        }

        private async Task CheckOutsourceStartAsync(DateOnly today, CancellationToken token)
        {
            var assignments = await _context.Assignments
                .Where(a => a.StartDate == today
                         && a.Status == "Planned"
                         && _context.Workshop.Any(w => w.Id == a.WorkshopId && w.WorkshopType == WorkshopType.Outsource))
                .ToListAsync(token);

            if (!assignments.Any()) return;

            foreach (var assignment in assignments)
            {
                assignment.UpdateStatus("InProgress");

                await _hubContext.Clients.All
                   .SendAsync("AssignmentStarted", new { Id = assignment.Id, Status = "InProgress" }, token);
            }
            _logger.LogInformation($"Started {assignments.Count} outsource assignments.");
        }
    }
}
