using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialSupplies.Command.UpdateInProgressByQcTransport
{
    public class UpdateInProgressByQcTransportCommandHandler : IRequestHandler<UpdateInProgressByQcTransportCommand, Result<Guid>>
    {
        private readonly IMaterialSupplyRepository _materialSupplyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IUserRepository _userRepository;

        public UpdateInProgressByQcTransportCommandHandler(IMaterialSupplyRepository materialSupplyRepository, IUnitOfWork unitOfWork, IMediator mediator, IMaterialRequestRepository materialRequestRepository,
            IUserRepository userRepository)
        {
            _materialSupplyRepository = materialSupplyRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _materialRequestRepository = materialRequestRepository;
            _userRepository = userRepository;
        }
        public async Task<Result<Guid>> Handle(UpdateInProgressByQcTransportCommand request, CancellationToken cancellationToken)
        {
            var materialSupply = await _materialSupplyRepository.GetByIdAsync(request.SupplyId);
            if (materialSupply == null)
                return Result<Guid>.Failure("Không tìm thấy phiếu cung cấp vật liệu.");

            var materialRequest = await _materialRequestRepository.GetByIdAsync(materialSupply.RequestId);
            if (materialRequest == null)
                return Result<Guid>.Failure("Không tìm thấy yêu cầu vật liệu tương ứng.");

            materialSupply.MarkAsInProgress();
            _materialSupplyRepository.Update(materialSupply);

            var qcTransport = await _userRepository.GetByIdAsync(request.QcTransportId);
            if (qcTransport == null)
                return Result<Guid>.Failure("Không tìm thấy người vận chuyển (QC Transport).");

            qcTransport.MarkAsNotQcTransport();
            _userRepository.Update(qcTransport);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new AddMaterialSupplyForQcWorkshopEvent(
                materialRequest.UserId,
                materialRequest.Id,
                materialSupply.MaterialId,
                materialSupply.Quantity));

            return Result<Guid>.Success(materialSupply.Id);
        }
    }
}
