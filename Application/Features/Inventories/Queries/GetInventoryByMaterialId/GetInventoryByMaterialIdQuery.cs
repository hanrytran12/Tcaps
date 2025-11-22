using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Inventories.Queries.GetInventoryByMaterialId
{
    public class GetInventoryByMaterialIdQuery : IRequest<InventoryHistoryDTO>
    {
        public Guid MaterialId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public GetInventoryByMaterialIdQuery(Guid materialId, int month, int year)
        {
            MaterialId = materialId;
            Month = month;
            Year = year;
        }
    }
}
