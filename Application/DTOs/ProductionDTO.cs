using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductionDTO
    {
        public Guid AssignId { get; private set; }
        public int Quantity { get; private set; }
        public DateOnly Date { get; private set; }
        public string Status { get; private set; } = string.Empty;
    }
}
