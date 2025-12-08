using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Materials.Queries
{
    public class GetAllMaterialQuery : IRequest<List<MaterialDTO>>
    {
        public string? MaterialName { get; set; }
    }
}
