using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class MaterialRequestForAssignmentDashboardDTO
    {
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public decimal QuantityRequest { get; set; }
        public decimal? QuantityResponse { get; set; }
        public decimal? QuantityActualAndStock { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
