using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Assignments.Queries.GetAssignmentsByStaffId
{
    public class GetAssignmentsByStaffIdQuery : IRequest<List<AssignForStaffDTO>>
    {
        public Guid StaffId { get; set; }
    }
}
