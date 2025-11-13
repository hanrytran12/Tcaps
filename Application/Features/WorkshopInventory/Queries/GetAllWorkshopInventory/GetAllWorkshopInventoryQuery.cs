using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetAllWorkshopInventory
{
    public class GetAllWorkshopInventoryQuery : IRequest<List<Domain.Entities.WorkshopInventory>>
    {
    }
}
