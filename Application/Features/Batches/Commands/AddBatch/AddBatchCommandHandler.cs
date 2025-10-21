using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Commands.AddBatch
{
    public class AddBatchCommandHandler : IRequestHandler<AddBatchCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBatchRepository _batchRepository;
        public AddBatchCommandHandler(IUnitOfWork unitOfWork, IBatchRepository batchRepository)
        {
            _unitOfWork = unitOfWork;
            _batchRepository = batchRepository;
        }

        public async Task<Result<Guid>> Handle(AddBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = new Batch(Guid.NewGuid(), request.ProductId, request.Code, request.Quantity, request.StartDate, request.EndDate);
            await _batchRepository.AddAsync(batch);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(batch.Id);
        }
    }
}
