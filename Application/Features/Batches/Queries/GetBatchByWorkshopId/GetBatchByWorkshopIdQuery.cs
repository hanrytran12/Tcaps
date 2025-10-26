using Domain.Entities;
using MediatR;

namespace Application.Features.Batches.Queries.GetBatchByWorkshopId
{
    public class GetBatchByWorkshopIdQuery : IRequest<List<Batch>>
    {
        public Guid WorkshopId { get; set; }

        public string? Status { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
    }
}
