using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByWorkshopId
{
    public class GetWorkshopInventoryByWorkshopIdQueryHandler : IRequestHandler<GetWorkshopInventoryByWorkshopIdQuery, List<Domain.Entities.WorkshopInventory>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetWorkshopInventoryByWorkshopIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Domain.Entities.WorkshopInventory>> Handle(GetWorkshopInventoryByWorkshopIdQuery request, CancellationToken cancellationToken)
        {
            return await _appDbContext.WorkshopInventory.Where(w => w.WorkshopId == request.WorkshopId).ToListAsync();
        }
    }
}
