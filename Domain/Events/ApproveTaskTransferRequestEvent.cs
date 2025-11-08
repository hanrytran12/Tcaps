using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class ApproveTaskTransferRequestEvent : INotification
    {
        public Guid TaskTransferRequestId { get; set; }
        public Guid QCTransportId { get; set; }
        public ApproveTaskTransferRequestEvent(Guid taskTransferRequestId, Guid qcTransportId)
        {
            TaskTransferRequestId = taskTransferRequestId;
            QCTransportId = qcTransportId;
        }
    }
}
