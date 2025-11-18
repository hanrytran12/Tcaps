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

namespace Application.Features.Batches.Queries.GetBatchesByStaffId
{
    public class GetBatchesByStaffIdQueryHandler : IRequestHandler<GetBatchesByStaffIdQuery, Result<List<BatchDTO>>>
    {
        private readonly IAppDbContext _context;

        public GetBatchesByStaffIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<BatchDTO>>> Handle(GetBatchesByStaffIdQuery request, CancellationToken cancellationToken)
        {
            var batches = await (from user in _context.Users.AsNoTracking()
                                 where user.Id == request.StaffId
                                 join a in _context.Assignments.AsNoTracking()
                                     on user.WorkshopId equals a.WorkshopId
                                 join b in _context.Batches.AsNoTracking()
                                     on a.BatchId equals b.Id
                                 join p in _context.Products.AsNoTracking()
                                     on b.ProductId equals p.Id
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
                                 // Dùng Distinct() để loại bỏ các Batch trùng lặp (vì 1 Batch có thể có nhiều Assignment)
                                 .Distinct()
                                 .ToListAsync(cancellationToken);

            if (!batches.Any())
            {
                var staffExists = await _context.Users.AnyAsync(u => u.Id == request.StaffId, cancellationToken);
                if (!staffExists)
                {
                    return Result<List<BatchDTO>>.Failure("Staff not found.");
                }
            }
            return Result<List<BatchDTO>>.Success(batches);
        }
    }
}
