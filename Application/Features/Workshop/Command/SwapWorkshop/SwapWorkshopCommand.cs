using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.Workshop.Command.SwapWorkshop
{
    public class SwapWorkshopCommand : IRequest<Result<string>>
    {
        public Guid WorkshopId1 { get; set; }
        public Guid WorkshopId2 { get; set; }
    }
}
