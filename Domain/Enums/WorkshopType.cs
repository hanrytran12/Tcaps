using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum WorkshopType
    {
        [Display(Name = "Xưởng thường")]
        Internal = 1,

        [Display(Name = "Xưởng khoán")]
        Outsource = 2
    }
}
