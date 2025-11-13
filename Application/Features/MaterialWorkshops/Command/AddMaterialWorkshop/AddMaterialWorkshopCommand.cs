using Application.Common;
using MediatR;

namespace Application.Features.MaterialWorkshops.Command.AddMaterialWorkshop
{
    public class AddMaterialWorkshopCommand : IRequest<Result<Guid>>
    {
        public Guid WorkshopId { get; set; }
        public Guid AssignId { get; set; }
        public int QuantitySend { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Image { get; set; }
    }
}
