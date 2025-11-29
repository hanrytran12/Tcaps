using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.MaterialRequest.Queries.GetMaterialRequestForQC
{
    public class GetMaterialRequestForQCQuery : IRequest<Result<List<MaterialRequestDTO>>>
    {
        public Guid QcId { get; set; }
        public string? Status { get; set; }
    }
}
