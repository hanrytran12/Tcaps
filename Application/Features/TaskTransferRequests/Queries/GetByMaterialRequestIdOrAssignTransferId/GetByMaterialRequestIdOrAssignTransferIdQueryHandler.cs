using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TaskTransferRequests.Queries.GetByMaterialRequestIdOrAssignTransferId
{
    public class GetByMaterialRequestIdOrAssignTransferIdQueryHandler : IRequestHandler<GetByMaterialRequestIdOrAssignTransferIdQuery, TaskTransferRequestDTO>
    {
        private readonly IAppDbContext _context;

        public GetByMaterialRequestIdOrAssignTransferIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<TaskTransferRequestDTO> Handle(GetByMaterialRequestIdOrAssignTransferIdQuery request, CancellationToken cancellationToken)
        {
            var query = from ttr in _context.TaskTransferRequests.AsNoTracking()

                            // LEFT JOIN với Batch (Giả định TaskTransferRequest có thể không có Batch nếu dữ liệu chưa hoàn chỉnh)
                        join batch in _context.Batches.AsNoTracking()
                            on ttr.BatchId equals batch.Id into batchGroup
                        from batchItem in batchGroup.DefaultIfEmpty()

                            // LEFT JOIN với Workshop
                        join workshop in _context.Workshop.AsNoTracking()
                            on ttr.WorkshopId equals workshop.Id into workshopGroup
                        from workshopItem in workshopGroup.DefaultIfEmpty()

                            // LEFT JOIN với User (QcTransport)
                        join user in _context.Users.AsNoTracking()
                            on ttr.QcTransportId equals user.Id into userGroup
                        from qcTransportUser in userGroup.DefaultIfEmpty()

                        join mr in _context.MaterialRequests.AsNoTracking()
                            on ttr.MaterialRequestId equals mr.Id into mrGroup
                        from mrItem in mrGroup.DefaultIfEmpty()

                        join m in _context.Materials.AsNoTracking()
                            on mrItem.MaterialId equals m.Id into materialGroup
                        from m in materialGroup.DefaultIfEmpty()

                        join assignTransfer in _context.AssignmentTransferRequests.AsNoTracking()
                            on ttr.AssignmentTransferId equals assignTransfer.Id into assignTransferGroup
                        from assignTransferItem in assignTransferGroup.DefaultIfEmpty()

                        where ttr.MaterialRequestId == request.RequestId || ttr.AssignmentTransferId == request.RequestId

                        // Ánh xạ trực tiếp sang DTO (Projection)
                        select new TaskTransferRequestDTO
                        {
                            Id = ttr.Id, // FIX LỖI MAPPING Ở ĐÂY
                            BatchId = ttr.BatchId,
                            BatchCode = batchItem.Code,
                            WorkshopId = ttr.WorkshopId,
                            WorkshopName = workshopItem.Name,
                            QcTransportId = ttr.QcTransportId,
                            QcTransportName = qcTransportUser.FullName,
                            MaterialRequestId = ttr.MaterialRequestId,
                            AssignmentTransferId = ttr.AssignmentTransferId,
                            MaterialName = m.Name,
                            QuantityRequest = mrItem == null ? 0 : (int)mrItem.QuantityRequest,
                            CompleteQuantity = assignTransferItem == null ? 0 : (int)assignTransferItem.CompletedQuantityReceive,
                            Status = ttr.Status,
                            Note = ttr.Note,
                            CreatedAt = ttr.CreatedAt,
                            ApprovedAt = ttr.ApprovedAt
                        };

            var dto = await query.FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
            {
                throw new NotFoundException("Task Transfer Request not found.");
            }

            return dto;
        }
    }
}
