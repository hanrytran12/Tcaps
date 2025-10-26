using Application.Common;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.AddMaterialRequest
{
    public class AddMaterialRequestCommand : IRequest<Result<Guid>>
    {
        public Guid MaterialId { get; set; }
        public Guid UserId { get; set; }
        public Guid BatchId { get; set; }
        public Guid AssignId { get; set; }
        public decimal QuantityRequest { get; set; }
        public string Note { get; set; } = string.Empty;
    }
}
