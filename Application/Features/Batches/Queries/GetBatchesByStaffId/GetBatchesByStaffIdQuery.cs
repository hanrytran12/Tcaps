using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Batches.Queries.GetBatchesByStaffId
{
    public class GetBatchesByStaffIdQuery : IRequest<List<StaffSummaryDashboardDTO>>
    {
        public Guid StaffId { get; set; }
    }
}
