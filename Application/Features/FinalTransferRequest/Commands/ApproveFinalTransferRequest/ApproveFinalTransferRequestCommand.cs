using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.FinalTransferRequest.Commands.ApproveFinalTransferRequest
{
    public class ApproveFinalTransferRequestCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public decimal QuantityFinalReceive { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
