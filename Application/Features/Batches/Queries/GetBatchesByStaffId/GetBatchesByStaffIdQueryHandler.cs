using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetBatchesByStaffId
{
    public class GetBatchesByStaffIdQueryHandler : IRequestHandler<GetBatchesByStaffIdQuery, List<BatchDTO>>
    {
        private readonly IAppDbContext _context;

        public GetBatchesByStaffIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<BatchDTO>> Handle(GetBatchesByStaffIdQuery request, CancellationToken cancellationToken)
        {
            var batches = await (from user in _context.Users.AsNoTracking()
                                 where user.Id == request.StaffId
                                 join a in _context.Assignments.AsNoTracking()
                                     on user.WorkshopId equals a.WorkshopId
                                 join b in _context.Batches.AsNoTracking()
                                     on a.BatchId equals b.Id
                                 join p in _context.Products.AsNoTracking()
                                     on b.ProductId equals p.Id
                                 join u in _context.Users.AsNoTracking()
                                     on b.UserId equals u.Id
                                 select new BatchDTO
                                 {
                                     BatchId = b.Id,
                                     UserId = b.UserId ?? Guid.Empty,
                                     LeadName = u.FullName,
                                     ProductCode = p.Code,
                                     ProductName = p.Name,
                                     Code = b.Code,
                                     Quantity = b.Quantity,
                                     StartDate = b.StartDate,
                                     EndDate = b.EndDate,
                                     Status = b.Status,
                                     CreatedAt = b.CreatedAt
                                 })
                                 .Distinct()
                                 .ToListAsync(cancellationToken);

            if (!batches.Any())
            {
                var staffExists = await _context.Users.AnyAsync(u => u.Id == request.StaffId, cancellationToken);
                if (!staffExists)
                {
                    throw new NotFoundException("Staff not found.");
                }
            }
            return batches;
        }
    }
}
