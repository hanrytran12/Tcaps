using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.ConfirmRequestFromLead
{
    public class ConfirmRequestFromLeadCommandHandler : IRequestHandler<ConfirmRequestFromLeadCommand, Result<Guid>>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IMediator _mediator;

        public ConfirmRequestFromLeadCommandHandler(IMaterialRequestRepository materialRequestRepository, IMediator mediator)
        {
            _materialRequestRepository = materialRequestRepository;
            _mediator = mediator;
        }
        public async Task<Result<Guid>> Handle(ConfirmRequestFromLeadCommand request, CancellationToken cancellationToken)
        {
            var materialRequest = await _materialRequestRepository.GetByIdAsync(request.MaterialRequest);

            if (materialRequest == null)
            {
                return Result<Guid>.Failure("Material request not found.");
            }

            materialRequest.MarkAsConfirmFromLead();
            _materialRequestRepository.Update(materialRequest);

            await _mediator.Publish(new ConfirmRequestFromLeadEvent(
                materialRequest.Id,
                materialRequest.UserId));
            return Result<Guid>.Success(materialRequest.Id);
        }
    }
}
