using Application.Common;
using Application.DTOs.Request;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Users.Queries.GetStaffDashboard
{
    public class GetStaffDashboardQuery : IRequest<StaffDashboardDTO>
    {
        [JsonIgnore]
        public Guid StaffId { get; set; }
        public Guid AssignId { get; set; }
    }
}
