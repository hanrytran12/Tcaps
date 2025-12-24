using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class TaskMetricsDTO
    {
        public decimal QuantityRequest { get; set; }
        public decimal QuantityCompleted { get; set; }
        public decimal QuantityError { get; set; }
        public decimal QuantityRework { get; set; }
    }
}
