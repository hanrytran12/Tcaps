using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByMaterialId
{
    public class GetWorkshopInventoryByMaterialIdQuery : IRequest<Domain.Entities.WorkshopInventory>
    {
        public Guid MaterialId { get; set; }
    }
}
