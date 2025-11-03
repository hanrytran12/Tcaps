using Application.Common;
using Application.DTOs.Request;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetStaffDashboard
{
    public class GetStaffDashboardQueryHandler : IRequestHandler<GetStaffDashboardQuery, Result<StaffDashboardDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetStaffDashboardQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Result<StaffDashboardDTO>> Handle(GetStaffDashboardQuery request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var user = await _appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.StaffId);

            if (user is null)
            {
                return Result<StaffDashboardDTO>.Success(new StaffDashboardDTO());
            }

            var currentAssignment = await _appDbContext.Assignments.AsNoTracking().FirstOrDefaultAsync(a => a.WorkshopId == user.WorkshopId && a.Status == "InProgress");

            if (currentAssignment is null)
            {
                return Result<StaffDashboardDTO>.Success(new StaffDashboardDTO());
            }

            var todayProduction = await _appDbContext.Productions.AsNoTracking()
                                                        .Where(p => p.AssignId == currentAssignment.Id
                                                        && p.UserId == user.Id
                                                        && p.Date == today)
                                                        .SumAsync(p => p.Quantity);

            var dashboard = new StaffDashboardDTO
            {
                TodayProducton = todayProduction,
                UnitPrice = currentAssignment.UnitPrice,
                EstimatedTodayIncome = todayProduction * currentAssignment.UnitPrice,
            };

            return Result<StaffDashboardDTO>.Success(dashboard);
        }
    }
}
