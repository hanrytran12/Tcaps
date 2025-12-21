using Application.Common;
using MediatR;

namespace Application.Features.Batches.Commands.AddBatch
{
    public class AddBatchCommand : IRequest<Result<Guid>>
    {
        public string CodeProduct { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
        public decimal Quantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
