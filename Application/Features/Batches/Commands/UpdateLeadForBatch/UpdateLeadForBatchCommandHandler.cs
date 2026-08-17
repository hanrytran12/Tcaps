using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Commands.UpdateLeadForBatch
{
    public class UpdateLeadForBatchCommandHandler : IRequestHandler<UpdateLeadForBatchCommand, Result<Guid>>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IUserRepository _userRepository;

        public UpdateLeadForBatchCommandHandler(IBatchRepository batchRepository, IUserRepository userRepository)
        {
            _batchRepository = batchRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<Guid>> Handle(UpdateLeadForBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(request.BatchId);
            if (batch is null)
            {
                return Result<Guid>.NotFound($"Không tìm thấy lô hàng với mã {request.BatchId}.", "batch_not_found");
            }

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user is null)
            {
                return Result<Guid>.NotFound($"Không tìm thấy người dùng với mã {request.UserId}.", "user_not_found");
            }

            batch.UpdateLeadForBatch(request.UserId);
            return Result<Guid>.Success(batch.Id);
        }
    }
}
