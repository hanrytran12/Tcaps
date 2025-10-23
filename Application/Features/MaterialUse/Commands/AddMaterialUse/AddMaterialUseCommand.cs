using Application.Common;
using MediatR;

namespace Application.Features.MaterialUse.Commands.AddMaterialUse
{
    public class AddMaterialUseCommand : IRequest<Result<Guid>>
    {
        public Guid MaterialId { get; set; }
        public Guid BatchId { get; set; }
        public Guid AssignId { get; set; }
        public decimal QuantityDivide { get; set; }
    }
}
