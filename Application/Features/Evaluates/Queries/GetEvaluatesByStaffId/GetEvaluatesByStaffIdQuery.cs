using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByStaffId
{
    public class GetEvaluatesByStaffIdQuery : IRequest<Result<List<EvaluateDTO>>>
    {
        public Guid StaffId { get; set; }
        public Guid AssignId { get; set; }
    }
}
