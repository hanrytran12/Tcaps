using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialWorkshops.Command.UpdateConfirmMaterialWorkshop
{
    public class UpdateConfirmMaterialWorkshopCommandHandler : IRequestHandler<UpdateConfirmMaterialWorkshopCommand, Result<Guid>>
    {
        private readonly IMaterialWorkshopRepository _materialWorkshopRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        public UpdateConfirmMaterialWorkshopCommandHandler(IMaterialWorkshopRepository materialWorkshopRepository, 
            IUnitOfWork unitOfWork, IMediator mediator, IUserRepository userRepository)
        {
            _materialWorkshopRepository = materialWorkshopRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _userRepository = userRepository;
        }
        public async Task<Result<Guid>> Handle(UpdateConfirmMaterialWorkshopCommand request, CancellationToken cancellationToken)
        {
            var materialWorkshop = await _materialWorkshopRepository.GetByIdAsync(request.Id);
            if (materialWorkshop == null)
                return Result<Guid>.Failure("Không tìm thấy phiếu vật liệu.");

            var supplier = await _userRepository.GetByIdAsync(materialWorkshop.SupplierId);

            materialWorkshop.Confirmed();
            materialWorkshop.Update(request.QuantityReceive);
            _materialWorkshopRepository.Update(materialWorkshop);

            if (supplier != null && supplier.Role == "QCTransport")
            {
                supplier.MarkAsNotQcTransport();
                _userRepository.Update(supplier);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new MaterialWorkshopConfirmEvent(
                materialWorkshop.WorkshopId,
                materialWorkshop.QuantitySend,
                materialWorkshop.QuantityReceive,
                materialWorkshop.ShipDate), cancellationToken);

            return Result<Guid>.Success(materialWorkshop.Id);
        }
    }
}
