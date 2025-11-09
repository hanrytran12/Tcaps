using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Assignments.Queries.GetAllAsignmentByQCId
{
    public class GetAllAssignmentByQCIdQuery : IRequest<List<AssignForStaffDTO>>
    {
        public Guid QcId { get; set; }
    }
}
