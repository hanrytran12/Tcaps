using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetGroupProgress
{
    public class GetGroupProgressQueryHandler : IRequestHandler<GetGroupProgressQuery, GroupProgressDTO>
    {
        private readonly IAppDbContext _context;

        public GetGroupProgressQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<GroupProgressDTO> Handle(GetGroupProgressQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user is null)
                throw new NotFoundException("Không tìm thấy người dùng");

            var workshop = await _context.Workshop.FindAsync(user.WorkshopId);

            // 🔹 Lấy assignment đang hoạt động
            var assignment = await _context.Assignments.FindAsync(request.AssignId);

            var reworkRequest = assignment.Status == "Reworking" ?
                await _context.ReworkRequests
                .Where(r => r.AssignmentId == assignment.Id
                    && r.Status != "PendingLead")
                .FirstOrDefaultAsync(cancellationToken) : null;

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            // Tổng sản lượng nhóm cho assignment này
            var totalProduction = await _context.Productions
                .Where(p => p.AssignId == request.AssignId)
                .SumAsync(p => p.Quantity);

            // 🔹 Tính tổng sản phẩm Unfixable của toàn nhóm trong assignment này
            var totalUnfixable = await (from a in _context.Assignments
                                        join p in _context.Productions on a.Id equals p.AssignId
                                        join e in _context.Evaluates on p.Id equals e.ProductionId
                                        join c in _context.ComponentDefects on e.Id equals c.EvaluateId
                                        where p.AssignId == request.AssignId && c.Status == "Unfixable"
                                        select c.Quantity).SumAsync(cancellationToken);

            // Tiến độ
            var target = assignment.Quantity; // 100

            var productionRework = 0;
            var targetRework = 0;
            var remainingRework = 0;

            if (assignment.Status == "Reworking" && reworkRequest != null)
            {
                targetRework = (int)reworkRequest.DefectiveQuantity;
                productionRework = Math.Max(totalProduction - target, 0);
                remainingRework = Math.Max(targetRework - productionRework, 0);
            }

            //var effectiveProduction = totalProduction - totalUnfixable; //105 - 5 = 100
            var remaining = Math.Max(target - totalProduction, 0); // 100 - 100

            // Số ngày còn lại (dùng DateOnly)
            var endDate = assignment.EndDate;
            if (assignment.Status == "Reworking" && reworkRequest?.EndDate != default)
            {
                endDate = reworkRequest.EndDate.Value;
            }
            var daysLeft = endDate != default ? Math.Max(endDate.DayNumber - today.DayNumber, 0) : 0;

            // Lấy danh sách nhân viên tham gia
            var staffNames = await (from p in _context.Productions
                                    join u in _context.Users on p.UserId equals u.Id
                                    where p.AssignId == assignment.Id
                                    select u.FullName)
                               .Distinct()
                               .ToListAsync(cancellationToken);

            var results = new GroupProgressDTO
            {
                WorkshopName = workshop.Name,
                Target = target,
                CurrentProduction = totalProduction,
                TotalUnfixable = totalUnfixable,
                RemainingProducts = remaining,
                TargetRework = targetRework,
                ReworkProduction = productionRework,
                RemainingRework = remainingRework,
                DaysLeft = daysLeft,
                Members = staffNames //cuong, dung
            };

            return results;
        }
    }
}
