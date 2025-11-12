using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.MaterialSupplies.Command.UpdateInProgressByQcTransport
{
    public class UpdateInProgressByQcTransportCommand : IRequest<Result<Guid>>
    {
        public Guid QcTransportId { get; set; }
        public Guid SupplyId { get; set; }
    }
}
