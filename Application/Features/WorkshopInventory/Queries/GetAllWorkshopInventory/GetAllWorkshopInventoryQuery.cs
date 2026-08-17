using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetAllWorkshopInventory
{
    public class GetAllWorkshopInventoryQuery : IRequest<List<Application.DTOs.Response.WorkshopInventoryDTO>>
    {
    }
}
