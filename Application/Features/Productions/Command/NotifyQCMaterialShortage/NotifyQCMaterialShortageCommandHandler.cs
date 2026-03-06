using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Productions.Command.NotifyQCMaterialShortage
{
    public class NotifyQCMaterialShortageCommandHandler : IRequestHandler<NotifyQCMaterialShortageCommand, Result>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMediator _mediator;

        public NotifyQCMaterialShortageCommandHandler(IAppDbContext appDbContext, IMediator mediator)
        {
            _appDbContext = appDbContext;
            _mediator = mediator;
        }

        public async Task<Result> Handle(NotifyQCMaterialShortageCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _appDbContext.Assignments
                .Where(a => a.Id == request.AssignId)
                .FirstOrDefaultAsync(cancellationToken);

            if (assignment == null)
                throw new NotFoundException("Không tìm thấy Assignment.");

            var material = await _appDbContext.Materials
                .Where(m => m.Id == request.MaterialId)
                .FirstOrDefaultAsync(cancellationToken);

            if (material == null)
                throw new NotFoundException("Không tìm thấy vật liệu.");

            var staff = await _appDbContext.Users
                .Where(u => u.Id == request.StaffId)
                .FirstOrDefaultAsync(cancellationToken);

            if (staff == null)
                throw new NotFoundException("Không tìm thấy nhân viên.");

            await _mediator.Publish(new MaterialShortageNotifiedEvent(
                request.AssignId,
                request.StaffId,
                request.MaterialId,
                material.Name,
                material.Unit,
                request.QuantityRemaining,
                assignment.WorkshopId), cancellationToken);

            return Result.Success();
        }
    }
}
