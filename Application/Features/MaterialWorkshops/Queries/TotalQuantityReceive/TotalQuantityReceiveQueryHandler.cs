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

            // Kiểm tra QC có tồn tại không
            var qc = await _context.Users.FindAsync(request.QcId);
            if (qc == null)
                throw new NotFoundException("Không tìm thấy QC.");

            // Kiểm tra Batch có tồn tại không
            var batch = await _context.Batches.FindAsync(request.BatchId);
            if (batch == null)
                throw new NotFoundException("Không tìm thấy Batch.");

            // Tìm assignment hiện tại của QC trong batch này
            var currentAssign = await _context.Assignments
                .Where(a => a.BatchId == request.BatchId
                         && a.WorkshopId == qc.WorkshopId)
                .FirstOrDefaultAsync(cancellationToken);

            if (currentAssign == null)
            {
                // Kiểm tra xem có assignment nào trong batch này không
                var hasAnyAssignment = await _context.Assignments
                    .AnyAsync(a => a.BatchId == request.BatchId, cancellationToken);

                if (!hasAnyAssignment)
                    throw new NotFoundException($"Không có Assignment nào cho Batch {batch.Code}.");
                else
                    throw new NotFoundException($"QC {qc.FullName} không thuộc workshop nào có assignment cho Batch {batch.Code}.");
            }

            // Nếu đây là bước đầu tiên (StepOrder = 1), không có xưởng trước
            if (currentAssign.StepOrder == 1)
            {
                return 0; // hoặc throw exception tùy logic nghiệp vụ
            }

            var prevAssign = await _context.Assignments
                .Where(a => a.BatchId == request.BatchId
                         && a.StepOrder == currentAssign.StepOrder - 1)
                .FirstOrDefaultAsync(cancellationToken);

            if (prevAssign == null)
                throw new NotFoundException($"Không tìm thấy Assignment của xưởng trước (StepOrder = {currentAssign.StepOrder - 1}).");

            // Lấy tổng QuantityReceive theo AssignId của xưởng trước
            var totalQuantity = await _context.MaterialWorkshops
                .Where(mw => mw.AssignId == prevAssign.Id)
                .SumAsync(mw => mw.QuantityReceive, cancellationToken);


            return totalQuantity;
        }
    }
}
