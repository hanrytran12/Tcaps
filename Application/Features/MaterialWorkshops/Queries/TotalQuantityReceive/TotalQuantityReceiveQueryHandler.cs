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
            var query = from batch in _context.Batches

                        join assign in _context.Assignments
                        on batch.Id equals assign.BatchId

                        join user in _context.Users
                        on assign.WorkshopId equals user.WorkshopId

                        join mw in _context.MaterialWorkshops
                        on user.WorkshopId equals mw.WorkshopId

                        where batch.Id == request.BatchId
                              && user.Id == request.QcId
                              && mw.AssignId == assign.Id
                        select mw.QuantityReceive;

            var totalQuantity = await query.SumAsync(cancellationToken);

            return Result<int>.Success(totalQuantity);
        }
    }
}
