using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class MaterialRequestDTO
    {
        public Guid Id { get; set; }
        public Guid MaterialId { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkshopId { get; set; }
        public string WorkshopName { get; set; }
        public Guid BatchId { get; set; }
        public Guid AssignId { get; set; }
        public decimal QuantityRequest { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string Type { get; set; }
        public string? NoteFromQC { get; set; } = null;
        public decimal? ActualReceivedQuantity { get; set; }
    }
}
