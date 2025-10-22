using MediatR;

namespace Application.Features.Users.Queries.GetStaffPerformance
{
    public class GetStaffPerformanceQuery : IRequest<List<StaffPerformanceDTO>>
    {
        public Guid? WorkshopId { get; set; }
    }
}
