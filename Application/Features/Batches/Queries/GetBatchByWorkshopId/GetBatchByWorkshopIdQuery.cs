using Application.DTOs.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Batches.Queries.GetBatchByWorkshopId
{
    public class GetBatchByWorkshopIdQuery : IRequest<List<BatchDTO>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public string? Status { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }

        public GetBatchByWorkshopIdQuery(Guid userId, string? status, DateOnly? fromDate, DateOnly? toDate)
        {
            UserId = userId;
            Status = status;
            FromDate = fromDate;
            ToDate = toDate;
        }
    }
}
