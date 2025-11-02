using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.Features.Assignments.Queries
{
    public class GetAssignmentsByStaffIdQuery : IRequest<List<AssignForStaffDTO>>
    {
        public Guid StaffId { get; set; }
    }
}
