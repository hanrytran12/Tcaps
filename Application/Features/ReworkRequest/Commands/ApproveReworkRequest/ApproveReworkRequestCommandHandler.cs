using Application.Common;
using Application.Interfaces;
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
                        where rr.Id == request.RequestId && rr.Status == "PendingLead"
                        join a in _appDbContext.Assignments on rr.AssignmentId equals a.Id
                        select new { a, rr };

            var queryInfo = await query.FirstOrDefaultAsync();

            var assignment = queryInfo.a;
            assignment.UpdateStatus("Reworking");

            var reworkRequest = queryInfo.rr;
            reworkRequest.ApproveRequest();

            return Result.Success();
        }
    }
}
