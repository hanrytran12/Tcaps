using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class NotificationForStaffEvent : INotification
    {
        public Guid UserId { get; set; }
        public Guid BatchId { get; set; }
        public decimal? ActualReceivedQuantity { get; set; }

        public NotificationForStaffEvent(Guid userId, Guid batchId, decimal? actualReceivedQuantity)
        {
            UserId = userId;
            BatchId = batchId;
            ActualReceivedQuantity = actualReceivedQuantity;
        }
    }
}
