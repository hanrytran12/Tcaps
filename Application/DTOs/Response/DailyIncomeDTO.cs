using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class DailyIncomeDTO
    {
        public int Day { get; set; }
        public decimal Total { get; set; }
        public int QuantityErrors { get; set; }
    }
}
