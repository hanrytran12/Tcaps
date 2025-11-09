using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.MaterialRequest.Queries.GetMaterialRequestForQC
{
    public class GetMaterialRequestForQCQuery : IRequest<Result<List<Domain.Entities.MaterialRequest>>>
    {
        public Guid QcId { get; set; }
        public string? Status { get; set; }
    }
}
