using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class UpdateComponentDefectDTO
    {
        public Guid? Id { get; set; }
        public string DefectType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Solution { get; set; } = string.Empty;
    }
}
