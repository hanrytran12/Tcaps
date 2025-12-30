using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetBatchesByStaffId
{
    public class GetBatchesByStaffIdQueryHandler : IRequestHandler<GetBatchesByStaffIdQuery, List<StaffSummaryDashboardDTO>>
    {
        private readonly IAppDbContext _context;

        public GetBatchesByStaffIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<StaffSummaryDashboardDTO>> Handle(GetBatchesByStaffIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Lấy WorkshopId của Staff hiện tại
            var staffInfo = await _context.Users.AsNoTracking()
                .Where(u => u.Id == request.StaffId)
                .Select(u => new { u.WorkshopId })
                .FirstOrDefaultAsync(cancellationToken);

            if (staffInfo == null || staffInfo.WorkshopId == null)
            {
                return new List<StaffSummaryDashboardDTO>();
            }

            // 2. Query chính
            var query = from a in _context.Assignments.AsNoTracking()
                        where a.WorkshopId == staffInfo.WorkshopId

                        // Join các bảng cha để lấy thông tin Batch
                        join b in _context.Batches.AsNoTracking() on a.BatchId equals b.Id
                        join p in _context.Products.AsNoTracking() on b.ProductId equals p.Id
                        join lead in _context.Users.AsNoTracking() on b.UserId equals lead.Id

                        select new StaffSummaryDashboardDTO
                        {
                            // Thông tin Batch (Giữ nguyên)
                            Batches = new BatchDTO
                            {
                                BatchId = b.Id,
                                UserId = b.UserId ?? Guid.Empty,
                                AssignmentId = a.Id,
                                LeadName = lead.FullName,
                                ProductCode = p.Code,
                                ProductName = p.Name,
                                Code = b.Code,
                                Quantity = a.Quantity,
                                StartDate = a.StartDate,
                                EndDate = a.EndDate,
                                Status = a.Status,
                                CreatedAt = b.CreatedAt,
                                UnitPrice = a.UnitPrice
                            },

                            // --- PHẦN SỬA ĐỔI ---
                            // Vì không có prod.User, ta phải JOIN thủ công tables Productions và Users tại đây
                            Assignments = (from prod in _context.Productions.AsNoTracking()
                                           join u in _context.Users.AsNoTracking() on prod.UserId equals u.Id
                                           where prod.AssignId == a.Id // Filter theo Assignment cha
                                           group prod by u.FullName into g // Group theo tên User
                                           select new StaffAssignmentDTO
                                           {
                                               StaffName = g.Key,                // Key chính là FullName
                                               Quantity = g.Sum(x => x.QuantityReceive) // Tính tổng Quantity
                                           }).ToList()
                        };

            return await query.ToListAsync(cancellationToken);
        }
    }
}
