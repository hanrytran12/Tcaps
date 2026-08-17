using Domain.Entities;
using MediatR;
using Application.DTOs.Response;

namespace Application.Features.Batches.Queries.GetBatchForLead
{
    public class GetBatchForLeadQuery : IRequest<List<BatchResponseDTO>>
    {
        public Guid UserId { get; set; }
    }
}
