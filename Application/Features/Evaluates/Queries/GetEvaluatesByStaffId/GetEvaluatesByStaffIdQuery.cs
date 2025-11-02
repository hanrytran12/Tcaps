using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByStaffId
{
    public class GetEvaluatesByStaffIdQuery : IRequest<List<Evaluate>>
    {
        public Guid StaffId { get; set; }
        public Guid AssignId { get; set; }
    }
}
