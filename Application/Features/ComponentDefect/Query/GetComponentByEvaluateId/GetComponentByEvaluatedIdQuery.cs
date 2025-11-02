using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Features.ComponentDefect.Query.GetComponentByEvaluateId
{
    public class GetComponentByEvaluatedIdQuery : IRequest<List<Domain.Entities.ComponentDefect>>
    {
        public Guid EvaluatedId { get; set; }
    }
}
