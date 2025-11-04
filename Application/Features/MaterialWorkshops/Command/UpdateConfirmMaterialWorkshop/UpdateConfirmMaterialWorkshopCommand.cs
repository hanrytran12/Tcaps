using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.MaterialWorkshops.Command.UpdateConfirmMaterialWorkshop
{
    public class UpdateConfirmMaterialWorkshopCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public int QuantityReceive { get; set; }
    }
}
