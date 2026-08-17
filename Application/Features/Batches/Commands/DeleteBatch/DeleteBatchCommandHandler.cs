using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Commands.DeleteBatch
{
    public class DeleteBatchCommandHandler : IRequestHandler<DeleteBatchCommand, Result>
    {
        private readonly IBatchRepository _batchRepository;
        public DeleteBatchCommandHandler(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public async Task<Result> Handle(DeleteBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(request.Id);
            if (batch is null)
            {
                return Result.NotFound($"Không tìm thấy lô hàng với mã {request.Id}.", "batch_not_found");
            }

            try
            {
                batch.MarkAsDeleted();
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure(ex.Message, "invalid_batch_state");
            }

            return Result.Success();
        }
    }
}
