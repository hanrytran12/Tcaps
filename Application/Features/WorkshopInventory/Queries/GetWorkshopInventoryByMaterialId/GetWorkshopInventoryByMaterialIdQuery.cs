using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByMaterialId
{
    public class GetWorkshopInventoryByMaterialIdQuery : IRequest<Application.DTOs.Response.WorkshopInventoryDTO>
    {
        public Guid MaterialId { get; set; }
    }
}
