using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Commands.UpdateBatch
{
    public class UpdateBatchCommandHandler : IRequestHandler<UpdateBatchCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBatchRepository _batchRepository;
        public UpdateBatchCommandHandler(IUnitOfWork unitOfWork, IBatchRepository batchRepository)
        {
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
        }

        public async Task<Result> Handle(UpdateBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(request.Id);
            if (batch is null)
            {
                return Result.Failure($"Batch with Id: {request.Id} not found.");
            }

            try
            {
                batch.UpdateDetails(request.Quantity, request.StartDate, request.EndDate);
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
