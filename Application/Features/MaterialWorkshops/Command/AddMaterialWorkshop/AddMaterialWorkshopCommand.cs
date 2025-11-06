using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using MediatR;

namespace Application.Features.MaterialWorkshops.Command.AddMaterialWorkshop
{
    public class AddMaterialWorkshopCommand : IRequest<Result<Guid>>
    {
        public Guid WorkshopId { get; set; }
        public int QuantitySend { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Image { get; set; }
    }
}
