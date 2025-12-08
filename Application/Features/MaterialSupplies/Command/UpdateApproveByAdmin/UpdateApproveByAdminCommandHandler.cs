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

namespace Application.Features.MaterialSupplies.Command.UpdateApproveByAdmin
{
    public class UpdateApproveByAdminCommandHandler : IRequestHandler<UpdateApproveByAdminCommand, Result<Guid>>
    {
        private readonly IMaterialSupplyRepository _materialSupplyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateApproveByAdminCommandHandler(IMaterialSupplyRepository materialSupplyRepository, IUserRepository userRepository, IUnitOfWork unitOfWork
            ,IMediator mediator)
        {
            _materialSupplyRepository = materialSupplyRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result<Guid>> Handle(UpdateApproveByAdminCommand request, CancellationToken cancellationToken)
        {
            var materialSupply = await _materialSupplyRepository.GetByIdAsync(request.MaterialSupplyId);
            if (materialSupply == null)
                throw new NotFoundException("Không tìm thấy Material Supply.");

            var qcTransport = await _userRepository.GetByIdAsync(materialSupply.SupplierId);
            if (qcTransport == null)
                throw new NotFoundException("Không tìm thấy người phụ trách vận chuyển (QC Transport).");

            materialSupply.MarkAsApprovedByAdmin();
            qcTransport.MarkAsQcTransport();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new AddMaterialSupplyForQcTransportEvent(
                    qcTransport.Id,
                    materialSupply.RequestId,
                    materialSupply.MaterialId,
                    materialSupply.QuantitySend));

            return Result<Guid>.Success(materialSupply.Id);
        }
    }
}
