using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TaskTransferRequests.Queries.GetTaskTransferRequestByQCTransportId
{
    public class GetTaskTransferRequestByQCTransportIdQueryHandler : IRequestHandler<GetTaskTransferRequestByQCTransportIdQuery, List<TaskTransferRequestDTO>>
    {
        private readonly ITaskTransferRequestRepository _taskTransferRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppDbContext _context;

        public GetTaskTransferRequestByQCTransportIdQueryHandler(ITaskTransferRequestRepository taskTransferRequestRepository, IUserRepository userRepository, IAppDbContext context)
        {
            _taskTransferRequestRepository = taskTransferRequestRepository;
            _userRepository = userRepository;
            _context = context;
        }
        public async Task<List<TaskTransferRequestDTO>> Handle(GetTaskTransferRequestByQCTransportIdQuery request, CancellationToken cancellationToken)
        {
            var qcTransport = await _userRepository.GetByIdAsync(request.QcTransportId);
            if (qcTransport == null || qcTransport.Role != "QCTransport")
            {
                throw new NotFoundException("Không tìm thấy QC vận chuyển này.");
            }

            var qcTransportName = qcTransport.FullName ?? string.Empty;

            var query = from ttr in _context.TaskTransferRequests.AsNoTracking()

                        where ttr.QcTransportId == request.QcTransportId

                        join batch in _context.Batches.AsNoTracking()
                            on ttr.BatchId equals batch.Id into batchGroup
                        from batchItem in batchGroup.DefaultIfEmpty()

                        join workshop in _context.Workshop.AsNoTracking()
                            on ttr.WorkshopId equals workshop.Id into workshopGroup
                        from workshopItem in workshopGroup.DefaultIfEmpty()

                        join mr in _context.MaterialRequests.AsNoTracking()
                            on ttr.MaterialRequestId equals mr.Id into mrGroup
                        from mrItem in mrGroup.DefaultIfEmpty()

                        join m in _context.Materials.AsNoTracking()
                            on mrItem.MaterialId equals m.Id into materialGroup
                        from m in materialGroup.DefaultIfEmpty()

                        join assignTransfer in _context.AssignmentTransferRequests.AsNoTracking()
                            on ttr.AssignmentTransferId equals assignTransfer.Id into assignTransferGroup
                        from assignTransferItem in assignTransferGroup.DefaultIfEmpty()


                        join currentAssign in _context.Assignments.AsNoTracking()
                            on assignTransferItem.AssignmentId equals currentAssign.Id into currentAssignGroup
                        from currentAssign in currentAssignGroup.DefaultIfEmpty()

                        join p in _context.Products.AsNoTracking()
                            on batchItem.ProductId equals p.Id into productGroup
                        from p in productGroup.DefaultIfEmpty()

                        let nextWorkshop =
                            currentAssign == null
                                ? null
                                : (
                                    from a in _context.Assignments
                                    where a.BatchId == currentAssign.BatchId
                                       && a.StepOrder > currentAssign.StepOrder
                                    orderby a.StepOrder
                                    join w in _context.Workshop on a.WorkshopId equals w.Id
                                    select w.Name
                                ).FirstOrDefault()

                        select new TaskTransferRequestDTO
                        {
                            Id = ttr.Id,
                            BatchId = ttr.BatchId,
                            BatchCode = batchItem.Code ?? string.Empty,

                            WorkshopId = ttr.WorkshopId,
                            WorkshopName = workshopItem.Name ?? string.Empty,

                            ProductCode = p != null ? p.Code : null,
                            ProductName = p != null ? p.Name : null,

                            QcTransportId = ttr.QcTransportId,
                            QcTransportName = qcTransportName,

                            MaterialRequestId = ttr.MaterialRequestId,
                            AssignmentTransferId = ttr.AssignmentTransferId,
                            MaterialName = m.Name ?? string.Empty,
                            QuantityRequest = mrItem == null ? 0 : (int)mrItem.QuantityRequest,
                            CompleteQuantity = assignTransferItem == null ? 0 : (int)assignTransferItem.CompletedQuantitySend,
                            Status = ttr.Status,
                            Note = ttr.Note,
                            CreatedAt = ttr.CreatedAt,
                            DateToGo = ttr.DateToGo,
                            ApprovedAt = ttr.ApprovedAt,
                            NextWorkshopName =
                                ttr.AssignmentTransferId == null
                                    ? null
                                    : nextWorkshop ?? "QC Gác Cổng"
                        };

            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(t => t.Status != null && t.Status.Equals(request.Status, StringComparison.OrdinalIgnoreCase));
            }

            var dtos = await query.ToListAsync(cancellationToken);

            if (!dtos.Any())
                throw new NotFoundException("Không tìm thấy yêu cầu chuyển giao nào cho QC này.");

            return dtos;
        }
    }
}
