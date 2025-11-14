using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class MaterialUseDTO
    {
        public Guid Id { get; set; }
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; }
        public decimal QuantityDivide { get; set; }
        public decimal QuantityStaffUse { get; set; }
        public decimal ReconciledQuantity { get; set; }
        public decimal QuantityRequest { get; set; }
        public DateOnly Date { get; set; }
    }
}
