using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
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

            if (assignment == null)
            {
                return Result<Guid>.Failure("Không tìm thấy công đoạn này.");
            }

            var qc = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.QcId);

            if (qc == null || qc.WorkshopId != assignment.WorkshopId)
            {
                return Result<Guid>.Failure("Người kiểm tra không hợp lệ.");
            }

            assignment.UpdateStatus("ReadyForTransfer");

            return Result<Guid>.Success(assignment.Id);
        }
    }
}
