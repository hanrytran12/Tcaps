using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class CreateMaterialRequestEvent : INotification
    {
        public Guid QC_Id { get; set; }
        public Guid BatchId { get; set; }
        public Guid AssignId { get; set; }

        public CreateMaterialRequestEvent(Guid qcId, Guid batchId, Guid assignId)
        {
            QC_Id = qcId;
            BatchId = batchId;
            AssignId = assignId;
        }
    }
}
