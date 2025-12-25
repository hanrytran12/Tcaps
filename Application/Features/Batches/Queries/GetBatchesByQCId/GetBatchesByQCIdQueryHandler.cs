using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetBatchesByQCId
{
    public class GetBatchesByQCIdQueryHandler : IRequestHandler<GetBatchesByQCIdQuery, List<BatchForQCDTO>>
    {
        private readonly IAppDbContext _context;

        public GetBatchesByQCIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<BatchForQCDTO>> Handle(GetBatchesByQCIdQuery request, CancellationToken cancellationToken)
        {
            // 1️⃣ Check QC tồn tại trước
            var qc = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.QcId, cancellationToken);

            if (qc == null)
                throw new NotFoundException("QC not found");

            // 2️⃣ Query batch (FIX: LEFT JOIN User)
            var batches = await (
                from a in _context.Assignments.AsNoTracking()
                where a.WorkshopId == qc.WorkshopId

                join b in _context.Batches.AsNoTracking()
                    on a.BatchId equals b.Id

                join p in _context.Products.AsNoTracking()
                    on b.ProductId equals p.Id

                join w in _context.Workshop.AsNoTracking()
                    on a.WorkshopId equals w.Id

                // 🔥 FIX: LEFT JOIN User (Lead có thể NULL)
                join u in _context.Users.AsNoTracking()
                    on b.UserId equals u.Id into leadGroup
                from u in leadGroup.DefaultIfEmpty()

                select new BatchForQCDTO
                {
                    Id = b.Id,
                    ProductId = p.Id,
                    ProductCode = p.Code,
                    UserId = b.UserId ?? Guid.Empty,
                    LeadName = u != null ? u.FullName : "Chưa phân công",
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
                }
            )
            .Distinct()
            .ToListAsync(cancellationToken);

            return batches!;
        }
    }
}
