using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class GroupProgressDTO
    {
        public string WorkshopName { get; set; } = string.Empty;
        public int Target { get; set; }
        public int CurrentProduction { get; set; }
        public int TotalUnfixable { get; set; }
        public int RemainingProducts { get; set; }
        public int TargetRework { get; set; }
        public int ReworkProduction { get; set; }
        public int RemainingRework { get; set; }
        public int DaysLeft { get; set; }
        public List<string> Members { get; set; } = new();
    }
}
