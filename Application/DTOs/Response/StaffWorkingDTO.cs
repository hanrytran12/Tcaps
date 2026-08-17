using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class StaffWorkingDTO
    {
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        public int QuantityWork { get; set; }
        public int QuantityError { get; set; }
    }
}
