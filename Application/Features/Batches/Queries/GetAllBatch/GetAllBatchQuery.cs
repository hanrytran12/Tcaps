using Domain.Entities;
using MediatR;
using Application.DTOs.Response;

namespace Application.Features.Batches.Queries.GetAllBatch
{
    public class GetAllBatchQuery : IRequest<List<BatchResponseDTO>>
    {
    }
}
