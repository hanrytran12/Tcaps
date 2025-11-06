using Application.DTOs.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Batches.Queries.GetBatchById
{
    public class GetBatchByIdQuery : IRequest<BatchDetailResponseDTO>
    {
        [JsonIgnore]
        public Guid BatchId { get; set; }

        public GetBatchByIdQuery(Guid batchId)
        {
            BatchId = batchId;
        }
    }
}
