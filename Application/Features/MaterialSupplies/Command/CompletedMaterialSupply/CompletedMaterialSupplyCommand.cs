using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.MaterialSupplies.Command.CompletedMaterialSupply
{
    public class CompletedMaterialSupplyCommand : IRequest<Result<Guid>>
    {
        public Guid QcId { get; set; }
        public Guid SupplyId { get; set; }
    }
}
