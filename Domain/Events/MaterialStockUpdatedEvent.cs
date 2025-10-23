using MediatR;

namespace Domain.Events
{
    public class MaterialStockUpdatedEvent : INotification
    {
        public string MaterialName { get; }
        public int QuantityChange { get; }
        public int NewStockQuantity { get; }

        public MaterialStockUpdatedEvent(string materialName, int newStockQuantity, int quantityChange)
        {
            MaterialName = materialName;
            NewStockQuantity = newStockQuantity;
            QuantityChange = quantityChange;
        }
    }
}
