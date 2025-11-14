using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class WorkshopInventoryForQCDTO
    {
        public Guid WorkshopInventoryId { get; set; }
        public Guid WorkshopId { get; set; }
        public Guid AssignId { get; set; }
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string BatchCode { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
