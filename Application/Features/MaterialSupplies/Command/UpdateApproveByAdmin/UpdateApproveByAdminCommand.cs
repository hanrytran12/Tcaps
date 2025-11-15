using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.MaterialSupplies.Command.UpdateApproveByAdmin
{
    public class UpdateApproveByAdminCommand : IRequest<Result<Guid>>
    {
        public Guid MaterialSupplyId { get; set; }
    }
}
