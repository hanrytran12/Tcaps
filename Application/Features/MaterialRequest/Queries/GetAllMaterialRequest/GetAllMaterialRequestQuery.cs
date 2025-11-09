using MediatR;

namespace Application.Features.MaterialRequest.Queries.GetAllMaterialRequest
{
    public class GetAllMaterialRequestQuery : IRequest<List<Domain.Entities.MaterialRequest>>
    {
    }
}
