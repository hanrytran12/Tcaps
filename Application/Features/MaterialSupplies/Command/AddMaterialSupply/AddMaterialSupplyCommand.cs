using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Request;
using MediatR;

namespace Application.Features.MaterialSupplies.Command.AddMaterialSupply
{
    public class AddMaterialSupplyCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid LeadId { get; set; }
        public Guid RequestId { get; set; }
        public Guid SupplierId { get; set; }
        public Guid WorkshopId { get; set; }
        public DateTime DateShip { get; set; }
        public Guid MaterialId { get; set; }
        public int QuantitySend { get; set; }
        public string Unit { get; set; }
    }
}
