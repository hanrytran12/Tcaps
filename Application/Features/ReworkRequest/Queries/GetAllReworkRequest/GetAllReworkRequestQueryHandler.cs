using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Queries.GetAllReworkRequest
{
    public class GetAllReworkRequestQueryHandler : IRequestHandler<GetAllReworkRequestQuery, List<Domain.Entities.ReworkRequest>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllReworkRequestQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Domain.Entities.ReworkRequest>> Handle(GetAllReworkRequestQuery request, CancellationToken cancellationToken)
        {
            return await _appDbContext.ReworkRequests.ToListAsync();
        }
    }
}
