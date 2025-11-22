using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Queries.GetReworkForDashboard
{
    public class GetReworkForDashboardQueryHandler : IRequestHandler<GetReworkForDashboardQuery, Domain.Entities.ReworkRequest>
    {
        private readonly IAppDbContext _appDbContext;

        public GetReworkForDashboardQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Domain.Entities.ReworkRequest> Handle(GetReworkForDashboardQuery request, CancellationToken cancellationToken)
        {
            var reworkRequest = await _appDbContext.ReworkRequests.Where(r => r.AssignmentId == request.AssignmentId).FirstOrDefaultAsync(cancellationToken);
            return reworkRequest;
        }
    }
}
