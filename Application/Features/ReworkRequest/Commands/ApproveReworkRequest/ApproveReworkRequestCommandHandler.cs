using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Commands.ApproveReworkRequest
{
    public class ApproveReworkRequestCommandHandler : IRequestHandler<ApproveReworkRequestCommand, Result>
    {
        private readonly IAppDbContext _appDbContext;

        public ApproveReworkRequestCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Result> Handle(ApproveReworkRequestCommand request, CancellationToken cancellationToken)
        {
            var query = from rr in _appDbContext.ReworkRequests
                        where rr.Id == request.RequestId
                        join a in _appDbContext.Assignments on rr.AssignmentId equals a.Id
                        select new { a, rr };

            var queryInfo = await query.FirstOrDefaultAsync();

            if (queryInfo == null)
            {
                throw new NotFoundException($"Không tìm thấy yêu cầu làm lại với ID: {request.RequestId}");
            }

            if (queryInfo.rr.Status != "PendingLead")
            {
                throw new ConflictException($"Yêu cầu này đang ở trạng thái '{queryInfo.rr.Status}', không thể từ chối được nữa.");
            }

            var assignment = queryInfo.a;
            assignment.UpdateStatus("Reworking");
            var reworkRequest = queryInfo.rr;
            if (!assignment.RequiresMaterialDelivery)
            {
                reworkRequest.InProgressRequest();
            }
            reworkRequest.ApproveRequest(request.DeliveryDate, request.EndDate, request.NextStepDeliveryDate);
            reworkRequest.AddDomainEvent(new ReworkRequestApprovedEvent(reworkRequest.QcId, request.DeliveryDate, request.EndDate));
            return Result.Success();
        }
    }
}
