using Application.Common.Exceptions;
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
            var reworkRequest = await _appDbContext.ReworkRequests.Where(r => r.Id == request.ReworkRequestId).FirstOrDefaultAsync(cancellationToken);
            if (reworkRequest is null)
            {
                throw new NotFoundException("ReworkRequest is not found");
            }

            return reworkRequest;
        }
    }
}
