using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetBatchesByQCId
{
    public class GetBatchesByQCIdQueryHandler : IRequestHandler<GetBatchesByQCIdQuery, BatchForQCDTO>
    {
        private readonly IAppDbContext _context;

        public GetBatchesByQCIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<BatchForQCDTO> Handle(GetBatchesByQCIdQuery request, CancellationToken cancellationToken)
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
                                 join w in _context.Workshop.AsNoTracking()
                                     on a.WorkshopId equals w.Id
                                 select new BatchForQCDTO
                                 {
                                     Id = b.Id,
                                     ProductId = p.Id,
                                     ProductCode = p.Code,
                                     UserId = b.UserId ?? Guid.Empty,
                                     LeadName = u.FullName,
                                     BatchCode = b.Code,
                                     Quantity = b.Quantity,
                                     StartDate = b.StartDate,
                                     EndDate = b.EndDate,
                                     Status = b.Status,
                                     CreatedAt = b.CreatedAt,

                                     Assignment = new AssignmentDTO
                                     {
                                         BatchId = b.Id,
                                         WorkshopId = a.WorkshopId,
                                         WorkshopName = w.Name,
                                         Quantity = a.Quantity,
                                         StartDate = a.StartDate,
                                         EndDate = a.EndDate,
                                         UnitPrice = a.UnitPrice,
                                         Status = a.Status,
                                         CreatedAt = a.CreatedAt
                                     }
                                 })
                                 .Distinct()
                                 .FirstOrDefaultAsync(cancellationToken);

            if (batches == null)
            {
                var qcExists = await _context.Users.AnyAsync(u => u.Id == request.QcId, cancellationToken);
                if (!qcExists)
                {
                    throw new NotFoundException("QC not found");
                }
            }
            return batches!;
        }
    }
}
