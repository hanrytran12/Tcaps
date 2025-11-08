using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.TaskTransferRequests.Queries.GetAllTaskTransferRequest
{
    public class GetAllTaskTransferRequestQueryHandler : IRequestHandler<GetAllTaskTransferRequestQuery, Result<List<TaskTransferRequest>>>
    {
        private readonly ITaskTransferRequestRepository _taskTransferRequestRepository;

        public GetAllTaskTransferRequestQueryHandler(ITaskTransferRequestRepository taskTransferRequestRepository)
        {
            _taskTransferRequestRepository = taskTransferRequestRepository;
        }
        public async Task<Result<List<TaskTransferRequest>>> Handle(GetAllTaskTransferRequestQuery request, CancellationToken cancellationToken)
        {
            var taskTransfers = await _taskTransferRequestRepository.GetAllAsync();
            if (taskTransfers == null || !taskTransfers.Any())
                return Result<List<TaskTransferRequest>>.Failure("Không tìm thấy yêu cầu chuyển giao nào cho QC này.");

            if (!string.IsNullOrEmpty(request.Status))
            {
                taskTransfers = taskTransfers.Where(t => t.Status == request.Status).ToList();
            }

            return Result<List<TaskTransferRequest>>.Success(taskTransfers.ToList());
        }
    }
}
