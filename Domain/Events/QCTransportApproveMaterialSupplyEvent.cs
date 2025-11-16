using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class QCTransportApproveMaterialSupplyEvent : INotification
    {
        public Guid QcTransportId { get; }
        public Guid MaterialSupplyId { get; }
        public QCTransportApproveMaterialSupplyEvent(Guid qcTransportId, Guid materialSupplyId)
        {
            QcTransportId = qcTransportId;
            MaterialSupplyId = materialSupplyId;
        }
    }
}
