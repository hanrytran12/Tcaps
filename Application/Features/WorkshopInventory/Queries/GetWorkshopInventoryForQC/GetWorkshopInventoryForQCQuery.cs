using Application.DTOs.Response;
using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryForQC
{
    public class GetWorkshopInventoryForQCQuery : IRequest<List<WorkshopInventoryForQCDTO>>
    {
        public Guid UserId { get; set; }
    }
}
