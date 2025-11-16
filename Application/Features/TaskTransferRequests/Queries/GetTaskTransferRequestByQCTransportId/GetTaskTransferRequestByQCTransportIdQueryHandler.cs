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
            var qcTransport = await _userRepository.GetByIdAsync(request.QcTransportId);
            if (qcTransport == null || qcTransport.Role != "QCTransport")
            {
                return Result<List<TaskTransferRequestDTO>>.Failure("Không tìm thấy QC vận chuyển này.");
            }

            var taskTransfers = await _taskTransferRequestRepository.GetByQcTransportIdAsync(request.QcTransportId);
            if (taskTransfers == null || !taskTransfers.Any())
                return Result<List<TaskTransferRequestDTO>>.Failure("Không tìm thấy yêu cầu chuyển giao nào cho QC này.");

            var batchIds = taskTransfers.Select(x => x.BatchId).Distinct().ToList();
            var workshopIds = taskTransfers.Select(x => x.WorkshopId).Distinct().ToList();
            var userIds = taskTransfers.Select(x => x.QcTransportId).Distinct().ToList();

            var batches = await _context.Batches
                .Where(x => batchIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

            var workshops = await _context.Workshop
                .Where(x => workshopIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            var users = await _context.Users
                .Where(x => userIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            // ⭐ CHUYỂN LIST THÀNH DICTIONARY – tìm nhanh O(1)
            var batchDict = batches.ToDictionary(x => x.Id, x => x);
            var workshopDict = workshops.ToDictionary(x => x.Id, x => x);

            // ⭐ Tạo DTO (không còn await trong vòng lặp)
            var dtos = new List<TaskTransferRequestDTO>();
            foreach (var item in taskTransfers)
            {
                batchDict.TryGetValue(item.BatchId, out var batch);
                workshopDict.TryGetValue(item.WorkshopId, out var workshop);

                var dto = new TaskTransferRequestDTO
                {
                    BatchId = item.BatchId,
                    BatchCode = batch?.Code ?? string.Empty,
                    WorkshopId = item.WorkshopId,
                    WorkshopName = workshop?.Name ?? string.Empty,
                    QcTransportId = item.QcTransportId,
                    QcTransportName = qcTransport?.FullName ?? string.Empty,
                    Status = item.Status,
                    Note = item.Note,
                    CreatedAt = item.CreatedAt,
                    ApprovedAt = item.ApprovedAt
                };
                dtos.Add(dto);
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                dtos = dtos
                    .Where(t => t.Status.Equals(request.Status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Result<List<TaskTransferRequestDTO>>.Success(dtos.ToList());
        }
    }
}
