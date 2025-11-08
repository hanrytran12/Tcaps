using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.TaskTransferRequests.Command.UpdateApproveTaskTransferRequest
{
    public class UpdateApproveTaskTransferRequestCommand : IRequest<Result<Guid>>
    {
        public Guid TaskTransferId { get; set; }
    }
}
