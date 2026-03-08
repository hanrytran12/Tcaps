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

namespace Application.Features.MaterialRequest.Commands.ConfirmRequestFromLead
{
    public class ConfirmRequestFromLeadCommandHandler : IRequestHandler<ConfirmRequestFromLeadCommand, Result<Guid>>
    {
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IMediator _mediator;

        public ConfirmRequestFromLeadCommandHandler(IMaterialRequestRepository materialRequestRepository, IMaterialRepository materialRepository, IMediator mediator)
        {
            _materialRequestRepository = materialRequestRepository;
            _materialRepository = materialRepository;
            _mediator = mediator;
        }
        public async Task<Result<Guid>> Handle(ConfirmRequestFromLeadCommand request, CancellationToken cancellationToken)
        {
            var materialRequest = await _materialRequestRepository.GetByIdAsync(request.MaterialRequest);

            if (materialRequest == null)
            {
                throw new NotFoundException("Không tìm thấy yêu cầu nguyên vật liệu.");
            }

            // Kiểm tra tồn kho NVL có đủ để cấp cho QC không
            var material = await _materialRepository.GetByIdAsync(materialRequest.MaterialId);
            if (material == null)
            {
                throw new NotFoundException($"Không tìm thấy nguyên vật liệu.");
            }

            // Số lượng thực tế cần cấp = QuantityRequest - QuantityFromStock (phần đã có sẵn ở xưởng)
            var quantityNeededFromWarehouse = materialRequest.QuantityRequest - materialRequest.QuantityFromStock;
            if (quantityNeededFromWarehouse > 0 && material.Quantity < quantityNeededFromWarehouse)
            {
                throw new BadRequestException(
                    $"Kho không đủ nguyên vật liệu \"{material.Name}\" để duyệt. " +
                    $"Cần thêm: {quantityNeededFromWarehouse} {material.Unit}, " +
                    $"Tồn kho hiện tại: {material.Quantity} {material.Unit}. " +
                    $"Vui lòng nhập thêm NVL vào kho trước khi duyệt.");
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
