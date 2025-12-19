using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.Workshop.Command.UpdateWorkshop
{
    public class UpdateWorkshopCommand : IRequest<Result<Guid>>
    {
        public Guid WorkshopId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
