using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.Productions.Command.UpdateProduction
{
    public class UpdateProductionCommand : IRequest<Result<Guid>>
    {
        public Guid ProductionId { get; set; }
        public int Quantity { get; set; }
    }
}
