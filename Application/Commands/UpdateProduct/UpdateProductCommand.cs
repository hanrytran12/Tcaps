using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? ImageURL { get; set; } = null;
    }
}
