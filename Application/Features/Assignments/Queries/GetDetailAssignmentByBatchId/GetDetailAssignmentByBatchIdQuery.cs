using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.GetDetailAssignmentByBatchId
{
    public class GetDetailAssignmentByBatchIdQuery : IRequest<Result<List<DashboardAssignmentDTO>>>
    {
        public Guid BatchId { get; set; }
    }
}
