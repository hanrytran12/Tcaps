using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.GetGroupProgress
{
    public class GetGroupProgressQueryHandler : IRequestHandler<GetGroupProgressQuery, Result<List<GroupProgressDTO>>>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IProductionRepository _productionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWorkshopRepository _workshopRepository;

        public GetGroupProgressQueryHandler(IAssignmentRepository assignmentRepository, IProductionRepository productionRepository
            ,IUserRepository userRepository,
            IWorkshopRepository workshopRepository)
        {
            _assignmentRepository = assignmentRepository;
            _productionRepository = productionRepository;
            _userRepository = userRepository;
            _workshopRepository = workshopRepository;
        }
        public async Task<Result<List<GroupProgressDTO>>> Handle(GetGroupProgressQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
                return Result<List<GroupProgressDTO>>.Failure("Không tìm thấy người dùng");

            var workshop = await _workshopRepository.GetByIdAsync(user.WorkshopId);

            // 🔹 Lấy tất cả assignment đang hoạt động của workshop đó
            var assignments = await _assignmentRepository.GetAssignmentsByWorkshopIdAsync(user.WorkshopId);
            assignments = assignments
                .Where(a => a.Status == "InProgress" || a.Status == "Pending")
                .ToList();

            if (!assignments.Any())
                return Result<List<GroupProgressDTO>>.Success(new List<GroupProgressDTO>());

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var results = new List<GroupProgressDTO>();

            foreach (var assignment in assignments)
            {
                // Tổng sản lượng nhóm cho assignment này
                var totalProduction = await _productionRepository.TotalProductionByAssignIdAsync(assignment.Id);

                // Tiến độ
                var target = assignment.Quantity;
                var progressPercent = target > 0 ? Math.Round((double)totalProduction * 100 / target, 2) : 0;
                var remaining = Math.Max(target - totalProduction, 0);

                // Số ngày còn lại (dùng DateOnly)
                var daysLeft = 0;
                if (assignment.EndDate != default)
                {
                    daysLeft = Math.Max(assignment.EndDate.DayNumber - today.DayNumber, 0);
                }

                // Lấy danh sách nhân viên tham gia
                var staffIds = await _productionRepository.GetAllStaffIdByAssignIdAsync(assignment.Id);
                var allUsers = await _userRepository.GetAllAsync();
                var staffNames = allUsers.Where(u => staffIds.Contains(u.Id)).Select(u => u.FullName).ToList();

                results.Add(new GroupProgressDTO
                {
                    WorkshopName = workshop.Name,
                    Target = target,
                    CurrentProduction = totalProduction,
                    ProgressPercent = progressPercent,
                    RemainingProducts = remaining,
                    DaysLeft = daysLeft,
                    Members = staffNames
                });
            }

            return Result<List<GroupProgressDTO>>.Success(results);
        }
    }
}
