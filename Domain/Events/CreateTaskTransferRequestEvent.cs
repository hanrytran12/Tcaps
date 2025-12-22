using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class CreateTaskTransferRequestEvent : INotification
    {
        public Guid BatchId { get; set; }
        public Guid WorkshopId { get; set; }
        public Guid QcTransportId { get; set; }
        public string? Note { get; set; }
        public DateTime DateToGo { get; set; }

        public CreateTaskTransferRequestEvent(Guid batchId, Guid workshopId, Guid qcTransportId, string note, DateTime dateToGo)
        {
            BatchId = batchId;
            WorkshopId = workshopId;
            QcTransportId = qcTransportId;
            Note = note;
            DateToGo = dateToGo;
        }
    }
}
