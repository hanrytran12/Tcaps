using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialWorkshops.Queries.TotalQuantityReceive
{
    public class TotalQuantityReceiveQueryHandler : IRequestHandler<TotalQuantityReceiveQuery, Result<int>>
    {
        private readonly IAppDbContext _context;

        public TotalQuantityReceiveQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<int>> Handle(TotalQuantityReceiveQuery request, CancellationToken cancellationToken)
        {
            var qc = await _context.Users.FindAsync(request.QcId);
            if (qc == null)
                return Result<int>.Failure("QC không tồn tại");

            var assignments = await _context.Assignments
                .Where(a => a.BatchId == request.BatchId && a.WorkshopId == qc.WorkshopId)
                .Select(a => a.Id)
                .ToListAsync();

            if (!assignments.Any())
                return Result<int>.Success(0);

            var totalQuantity = await _context.MaterialWorkshops
                .Where(mw => assignments.Contains(mw.AssignId))
                .SumAsync(mw => mw.QuantityReceive, cancellationToken);

            return Result<int>.Success(totalQuantity);
        }
    }
}
