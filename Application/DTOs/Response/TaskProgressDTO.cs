using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class TaskProgressDTO
    {
        public Guid AssignmentId { get; set; }
        public Guid BatchId { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public TaskMetricsDTO TaskMetricsDTO { get; set; } = new TaskMetricsDTO();
    }
}
