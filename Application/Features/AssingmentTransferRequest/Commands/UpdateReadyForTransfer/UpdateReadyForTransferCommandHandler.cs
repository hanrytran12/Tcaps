using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Commands.UpdateReadyForTransfer
{
    public class UpdateReadyForTransferCommandHandler : IRequestHandler<UpdateReadyForTransferCommand, Result<Guid>>
    {
        private readonly IAppDbContext _context;

        public UpdateReadyForTransferCommandHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Guid>> Handle(UpdateReadyForTransferCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.Id == request.AssignmentId);

            if (assignment is null)
            {
                throw new NotFoundException("Không tìm thấy công đoạn này.");
            }

            var qc = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.QcId);

            if (qc is null || qc.WorkshopId != assignment.WorkshopId)
            {
                throw new ConflictException("Người kiểm tra không hợp lệ.");
            }

            assignment.UpdateStatus("ReadyForTransfer");
            return Result<Guid>.Success(assignment.Id);
        }
    }
}