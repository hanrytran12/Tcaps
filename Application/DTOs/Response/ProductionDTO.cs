using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class ProductionDTO
    {
        public Guid Id { get; set; }        // thêm nếu dùng để submit lại
        public Guid AssignId { get; set; }
        public Guid? ReworkRequestId { get; set; }
        public string BatchCode { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public int Quantity { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
