using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialSupplies.Command.AddMaterialSupply
{
    public class AddMaterialSupplyCommandHandler : IRequestHandler<AddMaterialSupplyCommand, Result>
    {
        private readonly IAppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public AddMaterialSupplyCommandHandler(IAppDbContext context, IUnitOfWork unitOfWork, IMediator mediator)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result> Handle(AddMaterialSupplyCommand request, CancellationToken cancellationToken)
        {
            var lead = await _context.Users.FindAsync(request.LeadId);
            if (lead == null)
                throw new NotFoundException("Lead không tồn tại.");

            var materialRequest = await _context.MaterialRequests.FindAsync(request.RequestId);
            if (materialRequest == null)
                throw new NotFoundException("MaterialRequest không tồn tại.");

            var supplierId = request.SupplierId == Guid.Empty ? request.LeadId : request.SupplierId;

            var qcTransport = await _context.Users.FindAsync(supplierId);
            if (qcTransport == null)
                throw new NotFoundException("Không tìm thấy người phụ trách vận chuyển (QC Transport).");

            var supply = MaterialSupply.Create(
                request.RequestId,
                request.MaterialId,
                supplierId,
                request.WorkshopId,
                request.QuantitySend,
                request.Unit,
                request.DateShip,
                request.LeadId // để xác định ai là lead duyệt
            );

            await _context.MaterialSupplies.AddAsync(supply, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new AddMaterialSupplyForQcWorkshopEvent(
                materialRequest.UserId,
                request.RequestId,
                request.MaterialId,
                request.QuantitySend));

            return Result.Success();
        }
    }
}
