using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
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
                return Result<Guid>.Failure("Người dùng không tồn tại");


            var materialSupply = await _context.MaterialSupplies.FindAsync(request.SupplyId);
            if (materialSupply == null)
                return Result<Guid>.Failure("Không tìm thấy phiếu cung cấp vật liệu.");

            var materialRequest = await _context.MaterialRequests.FindAsync(materialSupply.RequestId);
            if (materialRequest == null)
                return Result<Guid>.Failure("Không tìm thấy yêu cầu vật liệu tương ứng.");

            if (qc.Id != materialRequest.UserId)
            {
                return Result<Guid>.Failure("Bạn không có quyền cập nhật");
            }

            materialSupply.MarkAsCompleted();
            _context.MaterialSupplies.Update(materialSupply);

            var materialUse = await _context.MaterialUse
                .FirstOrDefaultAsync(m => m.BatchId == materialRequest.BatchId
                                       && m.MaterialId == materialSupply.MaterialId);
            if (materialUse == null)
                return Result<Guid>.Failure("Không tìm thấy bản ghi sử dụng vật liệu cho lô hàng này.");

            materialUse.IncreaseQuantityStaffUse(materialSupply.Quantity);
            _context.MaterialUse.Update(materialUse);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new CompletedMaterialSupplyEvent(
                request.SupplyId,
                materialSupply.MaterialId,
                materialSupply.Quantity));
            return Result<Guid>.Success(materialSupply.Id);
        }
    }
}
