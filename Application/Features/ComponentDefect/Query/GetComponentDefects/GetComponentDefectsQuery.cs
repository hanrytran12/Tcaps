using Application.DTOs.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.ComponentDefects.Query.GetComponentDefects
{
    public class GetComponentDefectsQuery : IRequest<List<ComponentDefectsDTO>>
    {
        [JsonIgnore]
        public Guid QCId { get; set; }
        public string? Status { get; set; }
    }
}
