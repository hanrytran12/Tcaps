using MediatR;

namespace Domain.Events
{
    public class MaterialStockUpdatedEvent : INotification
    {
        public Guid? UserId { get; set; }
        public string MaterialName { get; }
        public int QuantityChange { get; }
        public int NewStockQuantity { get; }

        public MaterialStockUpdatedEvent(Guid? userId, string materialName, int newStockQuantity, int quantityChange)
        {
            UserId = userId;
            MaterialName = materialName;
            NewStockQuantity = newStockQuantity;
            QuantityChange = quantityChange;
        }
    }
}
