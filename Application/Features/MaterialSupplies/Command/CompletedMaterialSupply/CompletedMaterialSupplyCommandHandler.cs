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
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialSupplies.Command.CompletedMaterialSupply
{
    public class CompletedMaterialSupplyCommandHandler : IRequestHandler<CompletedMaterialSupplyCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IAppDbContext _context;

        public CompletedMaterialSupplyCommandHandler(IUnitOfWork unitOfWork, IMediator mediator, IAppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _context = context;
        }
        public async Task<Result<Guid>> Handle(CompletedMaterialSupplyCommand request, CancellationToken cancellationToken)
        {
            var qc = await _context.Users.FindAsync(request.QcId);
            if (qc == null)
                throw new NotFoundException("Người dùng không tồn tại");

            var today = DateTime.Now.Date;

            var materialSupply = await _context.MaterialSupplies.FindAsync(request.SupplyId);
            if (materialSupply == null)
                throw new NotFoundException("Không tìm thấy phiếu cung cấp vật liệu.");

            if (today < materialSupply.DateShip.Date)
            {
                throw new BadRequestException("Chưa tới ngày nhận vì chưa đến thời gian giao NVL.");
            }

            var materialRequest = await _context.MaterialRequests.FindAsync(materialSupply.RequestId);
            if (materialRequest == null)
                throw new NotFoundException("Không tìm thấy yêu cầu vật liệu tương ứng.");

            if (qc.Id != materialRequest.UserId)
            {
                throw new ForbiddenException("Bạn không có quyền cập nhật");
            }

            materialSupply.MarkAsCompleted(request.QuantityReceive, request.Note);
            _context.MaterialSupplies.Update(materialSupply);

            materialRequest.IncreaseQuantityActual(request.QuantityReceive);
            materialRequest.UpdateNoteFromQC(request.Note);

            var materialUse = await _context.MaterialUse
                .FirstOrDefaultAsync(m => m.BatchId == materialRequest.BatchId
                                       && m.MaterialId == materialSupply.MaterialId);
            if (materialUse == null)
                throw new NotFoundException("Không tìm thấy bản ghi sử dụng vật liệu cho lô hàng này.");

            materialUse.IncreaseQuantityRequest(materialSupply.QuantityReceive.Value);
            _context.MaterialUse.Update(materialUse);

            var supplier = await _context.Users.FindAsync(materialSupply.SupplierId);
            if (supplier == null)
                throw new NotFoundException("Không tìm thấy người vận chuyển (QC Transport).");

            if (supplier.Role == "QCTransport")
            {
                supplier.MarkAsNotQcTransport();
                _context.Users.Update(supplier);
            }

            var batch = await (from m in _context.MaterialRequests
                               join ms in _context.MaterialSupplies on m.Id equals ms.RequestId
                               join b in _context.Batches on m.BatchId equals b.Id
                               where ms.Id == materialSupply.Id
                               select b).FirstOrDefaultAsync();

            if (batch is null)
            {
                throw new NotFoundException("Không tìm thấy lô hàng.");
            }

            var material = await _context.Materials
                .Where(m => m.Id == materialSupply.MaterialId)
                .FirstOrDefaultAsync();

            material?.DecreaseQuantity(materialSupply.QuantitySend, batch.UserId ?? Guid.Empty);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new CompletedMaterialSupplyEvent(
                batch.UserId ?? Guid.Empty,
                materialSupply.MaterialId,
                batch.Code,
                materialSupply.QuantityReceive ?? 0));

            await _mediator.Publish(new NotificationForStaffEvent(
                qc.Id,
                batch.Id,
                request.QuantityReceive));
            return Result<Guid>.Success(materialSupply.Id);
        }
    }
}
