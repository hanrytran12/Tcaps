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

namespace Application.Features.TaskTransferRequests.Queries.GetAllTaskTransferRequest
{
    public class GetAllTaskTransferRequestQueryHandler : IRequestHandler<GetAllTaskTransferRequestQuery, Result<List<TaskTransferRequestDTO>>>
    {
        private readonly ITaskTransferRequestRepository _taskTransferRequestRepository;
        private readonly IAppDbContext _context;

        public GetAllTaskTransferRequestQueryHandler(ITaskTransferRequestRepository taskTransferRequestRepository, IAppDbContext context)
        {
            _taskTransferRequestRepository = taskTransferRequestRepository;
            _context = context;
        }
        public async Task<Result<List<TaskTransferRequestDTO>>> Handle(GetAllTaskTransferRequestQuery request, CancellationToken cancellationToken)
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
                            QuantityRequest = (int)mrItem.QuantityRequest,
                            Status = ttr.Status,
                            Note = ttr.Note,
                            CreatedAt = ttr.CreatedAt,
                            ApprovedAt = ttr.ApprovedAt
                        };

            // Áp dụng bộ lọc Status (nếu có)
            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(t => t.Status == request.Status);
            }

            // Thực thi truy vấn
            var dtos = await query.ToListAsync(cancellationToken);

            if (!dtos.Any())
                return Result<List<TaskTransferRequestDTO>>.Failure("Không tìm thấy yêu cầu chuyển giao nào.");

            return Result<List<TaskTransferRequestDTO>>.Success(dtos);
        }
    }
}
