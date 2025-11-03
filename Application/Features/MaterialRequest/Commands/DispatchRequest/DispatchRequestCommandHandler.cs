using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.DispatchRequest
{
    public class DispatchRequestCommandHandler : IRequestHandler<DispatchRequestCommand, Result>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IMaterialRequestRepository _materialRequestRepository;

        public DispatchRequestCommandHandler(IBatchRepository batchRepository, IMaterialRequestRepository materialRequestRepository)
        {
            _batchRepository = batchRepository;
            _materialRequestRepository = materialRequestRepository;
        }

        public async Task<Result> Handle(DispatchRequestCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByAssignmentIdAsync(request.AssignmentId);

            if (batch is null)
            {
                return Result.Failure("Không tìm thấy lô hàng");
            }

            foreach (var item in request.Items)
            {
                var materialRequest = Domain.Entities.MaterialRequest.Create(item.MaterialId, request.UserId, batch.Id, request.AssignmentId, item.Quantity, "Yêu cầu xuất kho từ Lead");
                await _materialRequestRepository.AddAsync(materialRequest);
            }

            return Result.Success();
        }
    }
}
