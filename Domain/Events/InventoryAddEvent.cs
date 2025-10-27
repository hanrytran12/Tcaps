using MediatR;

namespace Domain.Events
{
    public class InventoryAddEvent : INotification
    {
        public Guid MaterialId { get; }
        public int AddedQuantity { get; }
        public decimal Price { get; }
        public InventoryAddEvent(Guid materialId, int addedQuantity, decimal price)
        {
            MaterialId = materialId;
            AddedQuantity = addedQuantity;
            Price = price;
        }
    }
}
