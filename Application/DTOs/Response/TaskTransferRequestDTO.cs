using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class TaskTransferRequestDTO
    {
        public Guid Id { get; set; }
        public Guid BatchId { get; set; }
        public string BatchCode { get; set; }
        public Guid WorkshopId { get; set; }
        public string WorkshopName { get; set; }
        public Guid QcTransportId { get; set; }
        public string QcTransportName { get; set; }
        public Guid? MaterialRequestId { get; set; }
        public Guid? AssignmentTransferId { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly? ApprovedAt { get; set; }
    }
}
