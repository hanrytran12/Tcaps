using Application.DTOs.Response;
using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByWorkshopId
{
    public class GetWorkshopInventoryByWorkshopIdQuery : IRequest<List<WorkshopInventoryForExportDTO>>
    {
        public Guid WorkshopId { get; set; }

        public GetWorkshopInventoryByWorkshopIdQuery(Guid workshopId)
        {
            WorkshopId = workshopId;
        }
    }
}
