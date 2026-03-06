using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class FinalTransferRequestDTO
    {
        public Guid Id { get; set; }
        public Guid AssignTransferRequestId { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public decimal QuantityFinalSend { get; set; }
        public decimal? QuantityFinalReceive { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public string? ApprovedNote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ApprovedAt { get; set; }
    }
}
