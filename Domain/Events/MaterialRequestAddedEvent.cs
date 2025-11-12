using MediatR;

namespace Domain.Events
{
    public class MaterialRequestAddedEvent : INotification
    {
        public decimal Quantity { get; set; }
        public Guid AssignmentId { get; set; }
        public string MaterialName { get; set; }
        public string UnitMaterial { get; set; }

        public MaterialRequestAddedEvent(decimal quantity, Guid assignmentId, string materialName, string unitMaterial)
        {
            Quantity = quantity;
            AssignmentId = assignmentId;
            MaterialName = materialName;
            UnitMaterial = unitMaterial;
        }
    }
}
