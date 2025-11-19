using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByQCId
{
    public class GetEvaluatesByQCIdQuery : IRequest<Result<List<EvaluateDTO>>>
    {
        [JsonIgnore]
        public Guid QC_Id { get; set; }
        public string? Status { get; set; }
    }
}
