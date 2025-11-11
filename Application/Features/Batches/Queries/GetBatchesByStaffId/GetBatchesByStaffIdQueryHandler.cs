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
            var staff = await _context.Users.FindAsync(request.StaffId);
            if (staff == null)
                return Result<List<BatchDTO>>.Failure("Staff not found");

            var batches = await (
                from b in _context.Batches
                join a in _context.Assignments on b.Id equals a.BatchId
                join p in _context.Products on b.ProductId equals p.Id
                where a.WorkshopId == staff.WorkshopId
                select new BatchDTO
                {
                    BatchId = b.Id,
                    ProductName = p.Name,
                    Code = b.Code,
                    Quantity = b.Quantity,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    Status = b.Status
                }
            ).Distinct().ToListAsync(cancellationToken);

            return Result<List<BatchDTO>>.Success(batches);
        }
    }
}
