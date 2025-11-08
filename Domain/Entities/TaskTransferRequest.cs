using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Primitives;

namespace Domain.Entities
{
    public class TaskTransferRequest : Entity
    {
        public Guid BatchId { get; private set; }
        public Guid WorkshopId { get; private set; }
        public Guid QcTransportId { get; private set; }
        public string Status { get; private set; }
        public string? Note { get; private set; }
        public DateOnly CreatedAt { get; private set; }
        public DateOnly? ApprovedAt { get; private set; }
        public TaskTransferRequest(Guid id, Guid batchId, Guid workshopId, Guid qcTransportId, string? note) : base(id)
        {
            BatchId = batchId;
            WorkshopId = workshopId;
            QcTransportId = qcTransportId;
            Status = "Pending";
            Note = note;
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        }

        public static TaskTransferRequest Create(Guid batchId, Guid workshopId, Guid qcTransportId, string? note)
        {
            return new TaskTransferRequest(Guid.NewGuid(), batchId, workshopId, qcTransportId, note);
        }

        public void UpdateApproveStatus()
        {
            Status = "Approved";
            ApprovedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        }
    }
}
