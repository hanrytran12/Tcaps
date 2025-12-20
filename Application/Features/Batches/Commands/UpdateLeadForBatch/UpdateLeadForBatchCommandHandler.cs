using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Batches.Commands.UpdateLeadForBatch
{
    public class UpdateLeadForBatchCommandHandler : IRequestHandler<UpdateLeadForBatchCommand, Result<Guid>>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IMediator _mediator;

        public UpdateLeadForBatchCommandHandler(IBatchRepository batchRepository, IMediator mediator)
        {
            _batchRepository = batchRepository;
            _mediator = mediator;
        }
        public async Task<Result<Guid>> Handle(UpdateLeadForBatchCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByIdAsync(request.BatchId);
            if (batch is null)
            {
                throw new NotFoundException("Không tìm thấy lô hàng");
            }

            batch.UpdateLeadForBatch(request.UserId);

            await _mediator.Publish(new AddBatchEvent(
                request.UserId,
                batch.Code,
                batch.Quantity));

            return Result<Guid>.Success(batch.Id);
        }
    }
}
