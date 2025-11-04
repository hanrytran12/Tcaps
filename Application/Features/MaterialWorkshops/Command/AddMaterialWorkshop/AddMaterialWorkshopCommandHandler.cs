using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialWorkshops.Command.AddMaterialWorkshop
{
    public class AddMaterialWorkshopCommandHandler : IRequestHandler<AddMaterialWorkshopCommand, Result<Guid>>
    {
        private readonly IMaterialWorkshopRepository _materialWorkshopRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddMaterialWorkshopCommandHandler(IMaterialWorkshopRepository materialWorkshopRepository, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _materialWorkshopRepository = materialWorkshopRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(AddMaterialWorkshopCommand request, CancellationToken cancellationToken)
        {
            if (request.QuantitySend < 0 || request.QuantityReceive < 0)
                return Result<Guid>.Failure("Số lượng không hợp lệ.");

            var materialWorkshop = MaterialWorkshop.Create(
                request.WorkshopId,
                request.QuantitySend,
                request.QuantityReceive,
                request.Name,
                request.Unit,
                request.Image,
                "Pending");
            await _materialWorkshopRepository.AddAsync(materialWorkshop);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(materialWorkshop.Id);
        }
    }
}
