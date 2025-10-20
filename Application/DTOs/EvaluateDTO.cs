using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class EvaluateDTO
    {
        public Guid ProductionId { get; private set; }
        //userId là QC
        public Guid UserId { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string Note { get; private set; } = string.Empty;
        public string Image { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }
    }
}
