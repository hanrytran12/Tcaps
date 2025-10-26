using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByQCId
{
    public class GetEvaluatesByQCIdQuery : IRequest<List<EvaluateDTO>>
    {
        public Guid QC_Id { get; set; }
        public string Status { get; set; }
    }
}
