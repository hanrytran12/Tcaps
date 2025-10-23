using MediatR;

namespace Domain.Events
{
    public class InventoryAddEvent : INotification
    {
        public Guid MaterialId { get; }
        public int AddedQuantity { get; }
        public InventoryAddEvent(Guid materialId, int addedQuantity)
        {
            MaterialId = materialId;
            AddedQuantity = addedQuantity;
        }
    }
}
