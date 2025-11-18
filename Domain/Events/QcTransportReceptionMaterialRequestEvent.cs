using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class QcTransportReceptionMaterialRequestEvent : INotification
    {
        public Guid QcTransportId { get; set; }
        public Guid MaterialRequestId { get; set; }
        public Guid AssignId { get; set; }
        public QcTransportReceptionMaterialRequestEvent(Guid qcTransportId, Guid materialRequestId, Guid assignId)
        {
            QcTransportId = qcTransportId;
            MaterialRequestId = materialRequestId;
            AssignId = assignId;
        }
    }
}
