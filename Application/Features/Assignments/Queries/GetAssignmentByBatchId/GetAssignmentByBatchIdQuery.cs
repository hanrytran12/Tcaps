using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.GetAssignmentByBatchId
{
    public class GetAssignmentByBatchIdQuery : IRequest<Result<AssignForStaffDTO>>
    {
        public Guid BatchId { get; set; }
        public Guid StaffId { get; set; }
    }
}
