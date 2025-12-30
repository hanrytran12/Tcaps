using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Features.Assignments.Queries.NewFolder;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Queries.GetAssignmentForHistoryByBatchId
{
    public class GetAssignmentForHistoryByBatchIdQueryHandler : IRequestHandler<GetAssignmentForHistoryByBatchIdQuery, List<AssignmentHistoryDTO>>
    {
        private readonly IAppDbContext _context;

        public GetAssignmentForHistoryByBatchIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<AssignmentHistoryDTO>> Handle(GetAssignmentForHistoryByBatchIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == request.UserId)
                .Select(u => new { u.WorkshopId, u.Role })
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new NotFoundException("Người dùng không tồn tại.");

            var qcWorkshopId = user.WorkshopId;
            var userRole = user.Role;
            bool shouldFilterByWorkshop = userRole.Equals("QC", StringComparison.OrdinalIgnoreCase);

            if (shouldFilterByWorkshop && qcWorkshopId == null)
                throw new BadRequestException("Tài khoản QC này chưa được gán vào Xưởng nào.");

            var rawData = await (
                from assign in _context.Assignments.AsNoTracking()
                where assign.BatchId == request.BatchId &&
                    (!shouldFilterByWorkshop
                    || (qcWorkshopId != null && assign.WorkshopId == qcWorkshopId))
                join prod in _context.Productions.AsNoTracking()
                    on assign.Id equals prod.AssignId into prodsGroup
                from prod in prodsGroup.DefaultIfEmpty()
                join userStaff in _context.Users.AsNoTracking()
                    on (prod != null ? prod.UserId : Guid.Empty) equals userStaff.Id into userGroup
                from userStaff in userGroup.DefaultIfEmpty()
                where prod == null || (user != null && userStaff.Role.ToLower() == "staff")
                select new
                {
                    Assignment = assign,

                    ProductionId = prod != null ? prod.Id : (Guid?)null,
                    StaffId = userStaff != null ? userStaff.Id : Guid.Empty,
                    StaffName = userStaff != null ? userStaff.FullName : null,
                    QuantityWork = prod != null ? 
                        (prod.QuantityReceive > 0 ? prod.QuantityReceive : prod.QuantitySend) : 0,

                    QuantityError = prod == null
                        ? 0m
                        : _context.Evaluates
                            .Where(e => e.ProductionId == prod.Id)
                            .SelectMany(e => e.ComponentDefects)
                            .Where(cd => cd.Status == "Unfixable")
                            .Sum(cd => (decimal?)cd.Quantity) ?? 0
                })
                .ToListAsync(cancellationToken);

            if (!rawData.Any())
                return new List<AssignmentHistoryDTO>();

            var result = rawData
                .GroupBy(x => x.Assignment.Id)
                .Select(g =>
                {
                    var assignmentData = g.First().Assignment;

                    return new AssignmentHistoryDTO
                    {
                        AssignmentId = assignmentData.Id,
                        WorkshopId = assignmentData.WorkshopId,
                        StepOrder = assignmentData.StepOrder,
                        QuantityOrder = assignmentData.Quantity,
                        UnitPrice = assignmentData.UnitPrice,
                        StartDate = assignmentData.StartDate,
                        EndDate = assignmentData.EndDate,
                        DateCompleted = assignmentData.DateCompleted,
                        ExpectedDeliveryDate = assignmentData.ExpectedDeliveryDate,
                        Status = assignmentData.Status,

                        Items = g
                            .GroupBy(x => new { x.StaffId, x.StaffName })
                            .Select(sg => new StaffWorksingDTO
                            {
                                StaffId = sg.Key.StaffId,
                                StaffName = sg.Key.StaffName ?? "Chưa có dữ liệu.",
                                QuantityWork = sg.Sum(x => x.QuantityWork),
                                QuantityError = sg.Sum(x => (int)x.QuantityError)
                            })
                            .ToList()
                    };
                })
                .OrderBy(x => x.StepOrder)
                .ToList();

            return result;
        }
    }
}
