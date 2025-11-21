using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class ConfirmRequestFromLeadEvent : INotification
    {
        public Guid MaterialRequestId { get; }
        public Guid QcId { get; set; }
        public ConfirmRequestFromLeadEvent(Guid materialRequestId, Guid qcId)
        {
            MaterialRequestId = materialRequestId;
            QcId = qcId;
        }
    }
}
