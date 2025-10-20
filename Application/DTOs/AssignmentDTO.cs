using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AssignmentDTO
    {
        public Guid BatchId { get; private set; }
        public Guid WorkshopId { get; private set; }
        public int Quantity { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public DateOnly CreatedAt { get; private set; }
    }
}
