using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TaskTransferRequests.Queries.GetAllTaskTransferRequest
{
    public class GetAllTaskTransferRequestQueryHandler : IRequestHandler<GetAllTaskTransferRequestQuery, List<TaskTransferRequestDTO>>
    {
        private readonly ITaskTransferRequestRepository _taskTransferRequestRepository;
        private readonly IAppDbContext _context;

        public GetAllTaskTransferRequestQueryHandler(ITaskTransferRequestRepository taskTransferRequestRepository, IAppDbContext context)
        {
            _taskTransferRequestRepository = taskTransferRequestRepository;
            _context = context;
        }
        public async Task<List<TaskTransferRequestDTO>> Handle(GetAllTaskTransferRequestQuery request, CancellationToken cancellationToken)
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

                        join assignFromMR in _context.Assignments.AsNoTracking()
                            on mrItem.AssignId equals assignFromMR.Id into assignFromMRGroup
                        from assignFromMR in assignFromMRGroup.DefaultIfEmpty()

                        join m in _context.Materials.AsNoTracking()
                            on mrItem.MaterialId equals m.Id into materialGroup
                        from m in materialGroup.DefaultIfEmpty()

                        join assignTransfer in _context.AssignmentTransferRequests.AsNoTracking()
                            on ttr.AssignmentTransferId equals assignTransfer.Id into assignTransferGroup
                        from assignTransferItem in assignTransferGroup.DefaultIfEmpty()

                        join assignment in _context.Assignments.AsNoTracking()
                            on assignTransferItem.AssignmentId equals assignment.Id into assignmentGroup
                        from assignment in assignmentGroup.DefaultIfEmpty()

                        join p in _context.Products.AsNoTracking()
                            on batchItem.ProductId equals p.Id into productGroup
                        from p in productGroup.DefaultIfEmpty()

                        let nextWorkshop =
                            assignment == null
                                ? null
                                : (
                                    from a in _context.Assignments
                                    where a.StepOrder > assignment.StepOrder
                                    orderby a.StepOrder
                                    join w in _context.Workshop on a.WorkshopId equals w.Id
                                    select w.Name
                                  ).FirstOrDefault()

                        // Ánh xạ trực tiếp sang DTO (Projection)
                        select new TaskTransferRequestDTO
                        {
                            Id = ttr.Id, // FIX LỖI MAPPING Ở ĐÂY
                            BatchId = ttr.BatchId,
                            BatchCode = batchItem != null ? batchItem.Code : null,
                            WorkshopId = ttr.WorkshopId,
                            WorkshopName = workshopItem != null ? workshopItem.Name : null,
                            QcTransportId = ttr.QcTransportId,
                            QcTransportName = qcTransportUser != null ? qcTransportUser.FullName : null,
                            MaterialRequestId = ttr.MaterialRequestId,
                            AssignmentTransferId = ttr.AssignmentTransferId,
                            MaterialName = m != null ? m.Name : null,
                            QuantityRequest = mrItem == null ? 0 : (int)mrItem.QuantityRequest,
                            CompleteQuantity = assignTransferItem == null ? 0 : (int)assignTransferItem.CompletedQuantitySend,
                            Status = ttr.Status,
                            Note = ttr.Note,
                            CreatedAt = ttr.CreatedAt,
                            DateToGo = ttr.DateToGo,
                            ApprovedAt = ttr.ApprovedAt,
                            ProductCode = p != null ? p.Code : null,
                            ProductName = p != null ? p.Name : null,
                            NextWorkshopName =
                                ttr.AssignmentTransferId == null
                                    ? null
                                    : nextWorkshop ?? "QC Gác Cổng"
                        };

            // Áp dụng bộ lọc Status (nếu có)
            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(t => t.Status == request.Status);
            }

            // Thực thi truy vấn
            var dtos = await query.ToListAsync(cancellationToken);

            if (!dtos.Any())
                throw new NotFoundException("Không tìm thấy yêu cầu chuyển giao nào.");

            return dtos;
        }
    }
}
