using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class UpdateQuantityDefectiveEvent : INotification
    {
        public decimal QuantityReject { get; set; }
        public Guid QcId { get; set; }
        public string BatchCode { get; set; }

        public UpdateQuantityDefectiveEvent(decimal quantityReject, Guid qcId, string batchCode)
        {
            QuantityReject = quantityReject;
            QcId = qcId;
            BatchCode = batchCode;
        }
    }
}
