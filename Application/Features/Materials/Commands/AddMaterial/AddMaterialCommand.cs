using Application.Common;
using MediatR;

namespace Application.Features.Materials.Commands.AddMaterial
{
    public class AddMaterialCommand : IRequest<Result<Guid>>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
    }
}
