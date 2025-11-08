using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.TaskTransferRequests.Queries.GetTaskTransferRequestByQCTransportId
{
    public class GetTaskTransferRequestByQCTransportIdQuery : IRequest<Result<List<TaskTransferRequest>>>
    {
        public Guid QcTransportId { get; set; }
        public string? Status { get; set; }
    }
}
