using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkshopInventory.Queries.GetAllWorkshopInventory
{
    public class GetAllWorkshopinventoryQueryHandler : IRequestHandler<GetAllWorkshopInventoryQuery, List<Application.DTOs.Response.WorkshopInventoryDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllWorkshopinventoryQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Application.DTOs.Response.WorkshopInventoryDTO>> Handle(GetAllWorkshopInventoryQuery request, CancellationToken cancellationToken)
        {
            return await _appDbContext.WorkshopInventories
                .AsNoTracking()
                .Select(inventory => new Application.DTOs.Response.WorkshopInventoryDTO
                {
                    Id = inventory.Id,
                    WorkshopId = inventory.WorkshopId,
                    MaterialId = inventory.MaterialId,
                    Quantity = inventory.Quantity,
                    HoldingQuantity = inventory.HoldingQuantity,
                    AvailableQuantity = inventory.AvailableQuantity
                })
                .ToListAsync(cancellationToken);
        }
    }
}
