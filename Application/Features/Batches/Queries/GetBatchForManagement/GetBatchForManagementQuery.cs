using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Batches.Queries.GetBatchForManagement
{
    public class GetBatchForManagementQuery : IRequest<List<BatchDTO>>
    {
    }
}
