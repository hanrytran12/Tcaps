using MediatR;

namespace Domain.Events
{
    public class ReworkRequestApprovedEvent : INotification
    {
        public Guid QcId { get; set; }
        public DateOnly DeliveryDate { get; set; }
        public DateOnly EndDate { get; set; }

        public ReworkRequestApprovedEvent(Guid qcId, DateOnly deliveryDate, DateOnly endDate)
        {
            QcId = qcId;
            DeliveryDate = deliveryDate;
            EndDate = endDate;
        }
    }
}
