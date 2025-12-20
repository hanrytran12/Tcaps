using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Batches.Queries.GetBatchForLead
{
    public class GetBatchForLeadQuery : IRequest<List<BatchDTO>>
    {
        public Guid UserId { get; set; }
    }
}
