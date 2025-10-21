using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductionDTO
    {
        public Guid Id { get; set; }        // thêm nếu dùng để submit lại
        public Guid AssignId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
