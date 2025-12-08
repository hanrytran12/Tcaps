using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialWorkshops.Queries.TotalQuantityReceive
{
    public class TotalQuantityReceiveQueryHandler : IRequestHandler<TotalQuantityReceiveQuery, int>
    {
        private readonly IAppDbContext _context;

        public TotalQuantityReceiveQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(TotalQuantityReceiveQuery request, CancellationToken cancellationToken)
        {

            var currentAssign = await _context.Assignments //xưởng tiếp theo
                .Join(_context.Users,
                    a => a.WorkshopId,
                    u => u.WorkshopId,
                    (a, u) => new { a, u })
                .Where(x => x.a.BatchId == request.BatchId
                         && x.u.Id == request.QcId)
                .Select(x => x.a)
                .FirstOrDefaultAsync(cancellationToken);

            if (currentAssign == null)
                throw new NotFoundException("Không tìm thấy Assignment hiện tại.");

            var prevAssign = await _context.Assignments
                .Where(a => a.BatchId == request.BatchId
                         && a.StepOrder == currentAssign.StepOrder - 1)
                .FirstOrDefaultAsync(cancellationToken);

            if (prevAssign == null)
                throw new NotFoundException("Không tìm thấy Assignment của xưởng trước.");

            // Lấy tổng QuantityReceive theo AssignId của xưởng tiếp trước
            var totalQuantity = await _context.MaterialWorkshops
                .Where(mw => mw.AssignId == prevAssign.Id)
                .SumAsync(mw => mw.QuantityReceive, cancellationToken);


            return totalQuantity;
        }
    }
}
