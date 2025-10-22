using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Commands.DeleteBatch
{
    public class DeleteBatchCommandHandler : IRequestHandler<DeleteBatchCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBatchRepository _batchRepository;
        public DeleteBatchCommandHandler(IUnitOfWork unitOfWork, IBatchRepository batchRepository)
        {
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
        }

        public async Task<Result> Handle(DeleteBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(request.Id);
            if (batch is null)
            {
                return Result.Failure($"Batch with Id: {request.Id} not found.");
            }

            try
            {
                batch.MarkAsDeleted();
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure(ex.Message);
            }
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }
}
