using MediatR;

namespace Domain.Events
{
    public class MaterialShortageNotifiedEvent : INotification
    {
        public Guid AssignId { get; }
        public Guid StaffId { get; }
        public Guid MaterialId { get; }
        public string MaterialName { get; }
        public string MaterialUnit { get; }
        public decimal QuantityRemaining { get; }
        public Guid WorkshopId { get; }

        public MaterialShortageNotifiedEvent(
            Guid assignId,
            Guid staffId,
            Guid materialId,
            string materialName,
            string materialUnit,
            decimal quantityRemaining,
            Guid workshopId)
        {
            AssignId = assignId;
            StaffId = staffId;
            MaterialId = materialId;
            MaterialName = materialName;
            MaterialUnit = materialUnit;
            QuantityRemaining = quantityRemaining;
            WorkshopId = workshopId;
        }
    }
}
