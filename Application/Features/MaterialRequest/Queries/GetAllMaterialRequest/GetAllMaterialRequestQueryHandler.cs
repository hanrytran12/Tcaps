using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Queries.GetAllMaterialRequest
{
    public class GetAllMaterialRequestQueryHandler : IRequestHandler<GetAllMaterialRequestQuery, List<Domain.Entities.MaterialRequest>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllMaterialRequestQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Domain.Entities.MaterialRequest>> Handle(GetAllMaterialRequestQuery request, CancellationToken cancellationToken)
        {
            return await _appDbContext.MaterialRequests.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
