using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetBatchesByQCId
{
    public class GetBatchesByQCIdQueryHandler : IRequestHandler<GetBatchesByQCIdQuery, List<BatchDTO>>
    {
        private readonly IAppDbContext _context;

        public GetBatchesByQCIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<BatchDTO>> Handle(GetBatchesByQCIdQuery request, CancellationToken cancellationToken)
        {
            var batches = await (from user in _context.Users.AsNoTracking()
                                 where user.Id == request.QcId
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
                                     ProductName = p.Name,
                                     Code = b.Code,
                                     Quantity = b.Quantity,
                                     StartDate = b.StartDate,
                                     EndDate = b.EndDate,
                                     Status = b.Status
                                 })
                                 .Distinct()
                                 .ToListAsync(cancellationToken);

            if (!batches.Any())
            {
                var qcExists = await _context.Users.AnyAsync(u => u.Id == request.QcId, cancellationToken);
                if (!qcExists)
                {
                    throw new NotFoundException("QC not found");
                }
            }
            return batches;
        }
    }
}
