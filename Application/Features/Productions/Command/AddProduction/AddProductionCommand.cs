using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.Productions.Command.AddProduction
{
    public class AddProductionCommand : IRequest<Result<Guid>>
    {
        public Guid AssignId { get; set; }
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public DateOnly Date { get; set; }
    }
}
