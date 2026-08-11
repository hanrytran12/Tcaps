using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetReworkByAssignId
{
    public class GetReworkByAssignIdQuery : IRequest<Application.DTOs.Response.ReworkRequestResponseDTO>
    {
        public Guid AssignId { get; set; }
    }
}
