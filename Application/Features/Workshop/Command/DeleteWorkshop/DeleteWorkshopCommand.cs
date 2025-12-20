using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.Workshop.Command.DeleteWorkshop
{
    public class DeleteWorkshopCommand : IRequest<Result<Guid>>
    {
        public Guid WorkshopId { get; set; }
    }
}
