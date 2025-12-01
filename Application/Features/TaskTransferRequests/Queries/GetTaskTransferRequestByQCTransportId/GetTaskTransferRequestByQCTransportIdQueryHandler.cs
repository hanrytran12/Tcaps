using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TaskTransferRequests.Queries.GetTaskTransferRequestByQCTransportId
{
    public class GetTaskTransferRequestByQCTransportIdQueryHandler : IRequestHandler<GetTaskTransferRequestByQCTransportIdQuery, Result<List<TaskTransferRequestDTO>>>
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
        public async Task<Result<List<TaskTransferRequestDTO>>> Handle(GetTaskTransferRequestByQCTransportIdQuery request, CancellationToken cancellationToken)
        {
            // 1. Kiểm tra vai trò của QC Transport (Truy vấn bắt buộc)
            var qcTransport = await _userRepository.GetByIdAsync(request.QcTransportId);
            if (qcTransport == null || qcTransport.Role != "QCTransport")
            {
                return Result<List<TaskTransferRequestDTO>>.Failure("Không tìm thấy QC vận chuyển này.");
            }

            // Lấy Tên QC Transport một lần để sử dụng trong DTO (vì DTO cần FullName)
            var qcTransportName = qcTransport.FullName ?? string.Empty;

            // 2. TỐI ƯU HÓA: SINGLE QUERY (JOIN TaskTransferRequests, Batches, Workshops)
            var query = from ttr in _context.TaskTransferRequests.AsNoTracking()

                            // Lọc theo QC Transport ID ngay từ đầu
                        where ttr.QcTransportId == request.QcTransportId

                        // LEFT JOIN với Batch
                        join batch in _context.Batches.AsNoTracking()
                            on ttr.BatchId equals batch.Id into batchGroup
                        from batchItem in batchGroup.DefaultIfEmpty()

                            // LEFT JOIN với Workshop
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

                            // Ánh xạ trực tiếp sang DTO (Projection)
                        select new TaskTransferRequestDTO
                        {
                            Id = ttr.Id,
                            BatchId = ttr.BatchId,
                            // Sử dụng toán tử null-coalescing (??) để xử lý LEFT JOIN
                            BatchCode = batchItem.Code ?? string.Empty,

                            WorkshopId = ttr.WorkshopId,
                            WorkshopName = workshopItem.Name ?? string.Empty,

                            QcTransportId = ttr.QcTransportId,
                            // Sử dụng tên đã tải ở bước 1 (hoặc tiếp tục dùng JOIN nếu bạn không muốn dùng _userRepository)
                            QcTransportName = qcTransportName,

                            MaterialRequestId = ttr.MaterialRequestId,
                            AssignmentTransferId = ttr.AssignmentTransferId,
                            MaterialName = m.Name ?? string.Empty,
                            QuantityRequest = mrItem == null ? 0 : (int)mrItem.QuantityRequest,
                            CompleteQuantity = assignTransferItem == null ? 0 : (int)assignTransferItem.CompletedQuantity,
                            Status = ttr.Status,
                            Note = ttr.Note,
                            CreatedAt = ttr.CreatedAt,
                            ApprovedAt = ttr.ApprovedAt
                        };

            // 3. Áp dụng bộ lọc Status (nếu có)
            if (!string.IsNullOrEmpty(request.Status))
            {
                // Sử dụng Equals để dịch sang SQL và so sánh không phân biệt chữ hoa/thường (OrdinalIgnoreCase)
                query = query.Where(t => t.Status != null && t.Status.Equals(request.Status, StringComparison.OrdinalIgnoreCase));
            }

            // 4. Thực thi truy vấn
            var dtos = await query.ToListAsync(cancellationToken);

            if (!dtos.Any())
                return Result<List<TaskTransferRequestDTO>>.Failure("Không tìm thấy yêu cầu chuyển giao nào cho QC này.");

            return Result<List<TaskTransferRequestDTO>>.Success(dtos);
        }
    }
}
