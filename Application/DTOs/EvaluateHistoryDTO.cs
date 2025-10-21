using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class EvaluateHistoryDTO
    {
        public Guid ProductionId { get; set; }
        public string BatchCode { get; set; }
        public int QuantityProduced { get; set; }
        //userId là QC
        public Guid UserId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public DateOnly CreatedAt { get; set; }
    }
}
