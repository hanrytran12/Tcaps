using Application.DTOs.Response;
using MediatR;

namespace Application.Features.ComponentDefects.Query.GetComponentDefects
{
    public class GetComponentDefectsQuery : IRequest<List<ComponentDefectsDTO>>
    {
        public string? Status { get; set; }
    }
}
