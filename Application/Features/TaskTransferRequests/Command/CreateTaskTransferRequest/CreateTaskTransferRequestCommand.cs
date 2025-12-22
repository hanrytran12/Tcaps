using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.TaskTransferRequests.Command.CreateTaskTransferRequest
{
    public class CreateTaskTransferRequestCommand : IRequest<Result<Guid>>
    {
        public Guid BatchId { get; set; }
        public Guid WorkshopId { get; set; }
        public Guid QcTransportId { get; set; }
        public Guid? MaterialRequestId { get; set; }
        public Guid? AssignmentTransferId { get; set; }
        public string? Note { get; set; }
        public DateTime DateToGo { get; set; }
    }
}
