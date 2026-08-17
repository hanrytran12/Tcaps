using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class AssignmentHistoryDTO
    {
        public Guid AssignmentId { get; set; }
        public Guid WorkshopId { get; set; }
        public int? StepOrder { get; set; }
        public int QuantityOrder { get; set; }
        public decimal UnitPrice { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly? DateCompleted { get; set; }
        public DateOnly? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public ICollection<StaffWorkingDTO> Items { get; set; }
    }
}
