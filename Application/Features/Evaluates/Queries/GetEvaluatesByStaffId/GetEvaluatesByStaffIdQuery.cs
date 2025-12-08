using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByStaffId
{
    public class GetEvaluatesByStaffIdQuery : IRequest<List<EvaluateDTO>>
    {
        public Guid StaffId { get; set; }
        public Guid AssignId { get; set; }
    }
}
