using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Domain.Entities;
using MediatR;

namespace Application.Features.ComponentDefect.Query.GetComponentByEvaluateId
{
    public class GetComponentByEvaluatedIdQuery : IRequest<List<ComponentDefectsDTO>>
    {
        public Guid EvaluatedId { get; set; }
    }
}
