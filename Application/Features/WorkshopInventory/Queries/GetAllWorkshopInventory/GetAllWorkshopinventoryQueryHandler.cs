using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkshopInventory.Queries.GetAllWorkshopInventory
{
    public class GetAllWorkshopinventoryQueryHandler : IRequestHandler<GetAllWorkshopInventoryQuery, List<Domain.Entities.WorkshopInventory>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllWorkshopinventoryQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Domain.Entities.WorkshopInventory>> Handle(GetAllWorkshopInventoryQuery request, CancellationToken cancellationToken)
        {
            return await _appDbContext.WorkshopInventory.ToListAsync();
        }
    }
}
