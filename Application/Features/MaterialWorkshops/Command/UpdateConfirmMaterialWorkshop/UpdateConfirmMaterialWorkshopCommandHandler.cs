using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
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
        private readonly IAssignmentTransferRequestRepository _assignmentTransferRequestRepository;
        private readonly IAppDbContext _context;

        public UpdateConfirmMaterialWorkshopCommandHandler(IMaterialWorkshopRepository materialWorkshopRepository, 
            IUnitOfWork unitOfWork, IMediator mediator, IUserRepository userRepository, IAssignmentTransferRequestRepository assignmentTransferRequestRepository,
            IAppDbContext context)
        {
            _materialWorkshopRepository = materialWorkshopRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _userRepository = userRepository;
            _assignmentTransferRequestRepository = assignmentTransferRequestRepository;
            _context = context;
        }
        public async Task<Result<Guid>> Handle(UpdateConfirmMaterialWorkshopCommand request, CancellationToken cancellationToken)
        {
            var materialWorkshop = await _materialWorkshopRepository.GetByIdAsync(request.Id);
            if (materialWorkshop == null)
                return Result<Guid>.NotFound("Không tìm thấy phiếu vật liệu.");

            var supplier = await _userRepository.GetByIdAsync(materialWorkshop.SupplierId);

            materialWorkshop.Confirmed();
            materialWorkshop.Update(request.QuantityReceive);
            _materialWorkshopRepository.Update(materialWorkshop);

            var assignTransfer = await _assignmentTransferRequestRepository.GetByIdAsync(materialWorkshop.AssignmentTransferRequestId);
            if (assignTransfer is null)
            {
                throw new NotFoundException("Không tìm thấy đơn chuyển giao");
            }
            assignTransfer.MarkAsApproved();

            if (supplier != null && supplier.Role == "QCTransport")
            {
                supplier.MarkAsNotQcTransport();
                _userRepository.Update(supplier);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var assignment = await _context.Assignments.FindAsync(materialWorkshop.AssignId);
            if (assignment is null)
            {
                throw new NotFoundException("Không tìm thấy phân công.");
            }

            var batch = await _context.Batches.FindAsync(assignment.BatchId);
            if (batch is null)
            {
                throw new NotFoundException("Không tìm thấy lô hàng.");
            }

            await _mediator.Publish(new MaterialWorkshopConfirmEvent(
                materialWorkshop.WorkshopId,
                materialWorkshop.QuantitySend,
                materialWorkshop.QuantityReceive,
                batch.UserId,
                batch.Code), cancellationToken);

            return Result<Guid>.Success(materialWorkshop.Id);
        }
    }
}
