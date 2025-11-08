using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.TaskTransferRequests.Queries.GetTaskTransferRequestByQCTransportId
{
    public class GetTaskTransferRequestByQCTransportIdQueryHandler : IRequestHandler<GetTaskTransferRequestByQCTransportIdQuery, Result<List<TaskTransferRequest>>>
    {
        private readonly ITaskTransferRequestRepository _taskTransferRequestRepository;
        private readonly IUserRepository _userRepository;

        public GetTaskTransferRequestByQCTransportIdQueryHandler(ITaskTransferRequestRepository taskTransferRequestRepository, IUserRepository userRepository)
        {
            _taskTransferRequestRepository = taskTransferRequestRepository;
            _userRepository = userRepository;
        }
        public async Task<Result<List<TaskTransferRequest>>> Handle(GetTaskTransferRequestByQCTransportIdQuery request, CancellationToken cancellationToken)
        {
            var qcTransport = await _userRepository.GetByIdAsync(request.QcTransportId);
            if (qcTransport == null || qcTransport.Role != "QCTransport")
            {
                return Result<List<TaskTransferRequest>>.Failure("Không tìm thấy QC vận chuyển này.");
            }

            var taskTransfers = await _taskTransferRequestRepository.GetByQcTransportIdAsync(request.QcTransportId);
            if (taskTransfers == null || !taskTransfers.Any())
                return Result<List<TaskTransferRequest>>.Failure("Không tìm thấy yêu cầu chuyển giao nào cho QC này.");

            if (!string.IsNullOrEmpty(request.Status))
            {
                taskTransfers = taskTransfers
                    .Where(t => t.Status.Equals(request.Status, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Result<List<TaskTransferRequest>>.Success(taskTransfers.ToList());
        }
    }
}
