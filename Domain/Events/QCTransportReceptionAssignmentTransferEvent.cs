using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class QCTransportReceptionAssignmentTransferEvent : INotification
    {
        public Guid QcTransportId { get; }
        public Guid AssignmentTransferRequestId { get; }
        public Guid AssignId { get; }
        public QCTransportReceptionAssignmentTransferEvent(Guid qcId, Guid assignmentTransferRequestId, Guid assignId)
        {
            QcTransportId = qcId;
            AssignmentTransferRequestId = assignmentTransferRequestId;
            AssignId = assignId;
        }
    }
}
