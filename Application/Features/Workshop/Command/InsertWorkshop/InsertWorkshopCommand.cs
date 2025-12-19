using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.Workshop.Command.InsertWorkshop
{
    public class InsertWorkshopCommand : IRequest<Result<Guid>>
    {
        public Guid WorkshopId { get; set; }
        public Guid? PreviousWorkshopId { get; set; }
    }
}
