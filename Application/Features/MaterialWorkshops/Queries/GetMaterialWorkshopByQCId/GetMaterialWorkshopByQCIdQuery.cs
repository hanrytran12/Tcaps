using Application.DTOs.Response;
using MediatR;

namespace Application.Features.MaterialWorkshops.Queries.GetMaterialWorkshopByQCId
{
    public class GetMaterialWorkshopByQCIdQuery : IRequest<List<MaterialWorkshopDTO>>
    {
        public Guid QC_Id { get; set; }
        public Guid WorkshopId { get; set; }
    }
}
