using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByMaterialId
{
    public class GetWorkshopInventoryByMaterialIdQueryHandler : IRequestHandler<GetWorkshopInventoryByMaterialIdQuery, Application.DTOs.Response.WorkshopInventoryDTO>
    {
        private readonly IWorkshopInventoryRepository _repository;

        public GetWorkshopInventoryByMaterialIdQueryHandler(IWorkshopInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Application.DTOs.Response.WorkshopInventoryDTO> Handle(GetWorkshopInventoryByMaterialIdQuery request, CancellationToken cancellationToken)
        {
            var workshopInventory = await _repository.GetByMaterialIdAsync(request.MaterialId);
            if (workshopInventory is null)
            {
                throw new NotFoundException("Workshop inventory not found.");
            }

            return new Application.DTOs.Response.WorkshopInventoryDTO
            {
                Id = workshopInventory.Id,
                WorkshopId = workshopInventory.WorkshopId,
                MaterialId = workshopInventory.MaterialId,
                Quantity = workshopInventory.Quantity,
                HoldingQuantity = workshopInventory.HoldingQuantity,
                AvailableQuantity = workshopInventory.AvailableQuantity
            };
        }
    }
}
