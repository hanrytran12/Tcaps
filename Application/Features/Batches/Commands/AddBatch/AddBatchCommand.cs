using Application.Common;
using MediatR;

namespace Application.Features.Batches.Commands.AddBatch
{
    public class AddBatchCommand : IRequest<Result<Guid>>
    {
        public Guid ProductId { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
