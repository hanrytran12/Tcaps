using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Domain.Events
{
    public class CompletedMaterialSupplyEvent : INotification
    {
        public Guid UserId  { get; set; }
        public Guid MaterialId { get; set; }
        public string BatchCode { get; set; }
        public int Quantity { get; set; }
        public CompletedMaterialSupplyEvent(Guid userId, Guid materialId, string batchCode, int quantity)
        {
            UserId = userId;
            MaterialId = materialId;
            BatchCode = batchCode;
            Quantity = quantity;
        }
    }
}
