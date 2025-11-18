using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetBatchesByQCId
{
    public class GetBatchesByQCIdQueryHandler : IRequestHandler<GetBatchesByQCIdQuery, Result<List<BatchDTO>>>
    {
        private readonly IAppDbContext _context;

        public GetBatchesByQCIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<BatchDTO>>> Handle(GetBatchesByQCIdQuery request, CancellationToken cancellationToken)
        {
            var batches = await (from user in _context.Users.AsNoTracking()
                                 where user.Id == request.QcId

                                 join a in _context.Assignments.AsNoTracking()
                                     on user.WorkshopId equals a.WorkshopId

                                 join b in _context.Batches.AsNoTracking()
                                     on a.BatchId equals b.Id

                                 join p in _context.Products.AsNoTracking()
                                     on b.ProductId equals p.Id

                                 // 5. Projection ra DTO
                                 select new BatchDTO
                                 {
                                     BatchId = b.Id,
                                     ProductName = p.Name,
                                     Code = b.Code,
                                     Quantity = b.Quantity,
                                     StartDate = b.StartDate,
                                     EndDate = b.EndDate,
                                     Status = b.Status
                                 })
                                 // Dùng Distinct() để loại bỏ các Batch trùng lặp (nếu 1 Batch có nhiều Assignment trong cùng Workshop)
                                 .Distinct()
                                 .ToListAsync(cancellationToken);

            if (!batches.Any())
            {
                var qcExists = await _context.Users.AnyAsync(u => u.Id == request.QcId, cancellationToken);
                if (!qcExists)
                {
                    return Result<List<BatchDTO>>.Failure("QC not found");
                }
            }

            return Result<List<BatchDTO>>.Success(batches);
        }
    }
}
