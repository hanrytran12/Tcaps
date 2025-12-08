using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Batches.Queries.GetBatchesByStaffId
{
    public class GetBatchesByStaffIdQuery : IRequest<List<BatchDTO>>
    {
        public Guid StaffId { get; set; }
    }
}
