using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByWorkshopId
{
    public class GetWorkshopInventoryByWorkshopIdQuery : IRequest<List<Domain.Entities.WorkshopInventory>>
    {
        public Guid WorkshopId { get; set; }

        public GetWorkshopInventoryByWorkshopIdQuery(Guid workshopId)
        {
            WorkshopId = workshopId;
        }
    }
}
