using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class EvaluateDTO
    {
        public Guid Id { get; set; }
        public Guid ProductionId { get; set; }
        public int QuantityError { get; set; }
        public string Note { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string Status { get; set; }

        public List<ComponentDefectsDTO> Defects { get; set; } = new();
    }
}
