using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Commands.UpdateBatch
{
    public class UpdateBatchCommandHandler : IRequestHandler<UpdateBatchCommand, Result>
    {
        private readonly IBatchRepository _batchRepository;
        public UpdateBatchCommandHandler(IBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public async Task<Result> Handle(UpdateBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(request.Id);
            if (batch is null)
            {
                throw new NotFoundException($"Batch with Id: {request.Id} not found.");
            }

            try
            {
                batch.UpdateDetails(request.Quantity, request.StartDate, request.EndDate);
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure(ex.Message);
            }
            return Result.Success();
        }
    }
}
