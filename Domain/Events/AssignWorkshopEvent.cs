using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class AssignWorkshopEvent : INotification
    {
        public Guid UserId { get; set; }
        public string BatchCode { get; set; } = string.Empty;

        public AssignWorkshopEvent(Guid userId, string batchCode)
        {
            UserId = userId;
            BatchCode = batchCode;
        }
    }
}
