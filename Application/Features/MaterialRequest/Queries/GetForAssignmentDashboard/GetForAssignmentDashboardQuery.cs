using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.MaterialRequest.Queries.GetForAssignmentDashboard
{
    public class GetForAssignmentDashboardQuery : IRequest<List<MaterialRequestForAssignmentDashboardDTO>>
    {
        public Guid AssignmentId { get; set; }
    }
}
