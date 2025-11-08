using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Materials.Queries.GetAllMaterialToWatch
{
    public class GetAllMaterialToWatchQuery : IRequest<List<MaterialToWatchDTO>>
    {
    }
}
