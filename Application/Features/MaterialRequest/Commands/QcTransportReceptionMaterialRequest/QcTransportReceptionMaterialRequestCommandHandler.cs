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

namespace Application.Features.MaterialRequest.Commands.QcTransportReceptionMaterialRequest
{
    public class QcTransportReceptionMaterialRequestCommandHandler : IRequestHandler<QcTransportReceptionMaterialRequestCommand, Result<Guid>>
    {
        private readonly IMaterialRequestRepository _repository;
        private readonly IUnitOfWork _unitOfWorks;
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        public QcTransportReceptionMaterialRequestCommandHandler(IMaterialRequestRepository repository, IUnitOfWork unitOfWorks, IMediator mediator, IUserRepository userRepository)
        {
            _repository = repository;
            _unitOfWorks = unitOfWorks;
            _mediator = mediator;
            _userRepository = userRepository;
        }
        public async Task<Result<Guid>> Handle(QcTransportReceptionMaterialRequestCommand request, CancellationToken cancellationToken)
        {
            var materialRequest = await _repository.GetByIdAsync(request.MaterialRequestId);

            if (materialRequest == null)
            {
                throw new NotFoundException("Material request not found.");
            }

            if (materialRequest.Status == "QCTransportReception")
            {
                throw new BadRequestException("Yêu cầu đã được tiếp nhận");
            }

            var qcTransport = await _userRepository.GetByIdAsync(request.QcTransportId);
            if (qcTransport == null)
                throw new NotFoundException("Không tìm thấy người vận chuyển (QC Transport).");

            materialRequest.MarkAsReception();
            _repository.Update(materialRequest);

            await _unitOfWorks.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new QcTransportReceptionMaterialRequestEvent(
                request.QcTransportId,
                request.MaterialRequestId,
                materialRequest.AssignId));

            return Result<Guid>.Success(materialRequest.Id);
        }
    }
}
