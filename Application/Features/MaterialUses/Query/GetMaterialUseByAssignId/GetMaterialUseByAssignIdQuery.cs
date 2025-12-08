using Application.DTOs.Response;
using MediatR;

namespace Application.Features.MaterialUses.Query.GetMaterialUseByAssignId
{
    public class GetMaterialUseByAssignIdQuery : IRequest<List<MaterialUseDTO>>
    {
        public Guid AssignId { get; set; }
        public string? MaterialName { get; set; }
    }
}
