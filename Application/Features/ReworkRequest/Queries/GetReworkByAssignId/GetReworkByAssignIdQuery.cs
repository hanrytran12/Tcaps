using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.ReworkRequest.Queries.GetReworkByAssignId
{
    public class GetReworkByAssignIdQuery : IRequest<Result<Domain.Entities.ReworkRequest>>
    {
        public Guid AssignId { get; set; }
    }
}
