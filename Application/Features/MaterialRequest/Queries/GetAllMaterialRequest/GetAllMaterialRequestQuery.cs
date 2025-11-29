using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.MaterialRequest.Queries.GetAllMaterialRequest
{
    public class GetAllMaterialRequestQuery : IRequest<Result<List<MaterialRequestDTO>>>
    {
    }
}
