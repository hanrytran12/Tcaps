using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Queries.GetRequestById
{
    public class GetRequestByIdQueryHandler : IRequestHandler<GetRequestByIdQuery, Domain.Entities.ReworkRequest>
    {
        private readonly IAppDbContext _appDbContext;

        public GetRequestByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Domain.Entities.ReworkRequest> Handle(GetRequestByIdQuery request, CancellationToken cancellationToken)
        {
            return await _appDbContext.ReworkRequests.Where(r => r.Id == request.ReworkRequestId).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
