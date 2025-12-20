using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.Batches.Commands.UpdateLeadForBatch
{
    public class UpdateLeadForBatchCommand : IRequest<Result<Guid>>
    {
        public Guid BatchId { get; set; }
        public Guid UserId { get; set; }
    }
}
