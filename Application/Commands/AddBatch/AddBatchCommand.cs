using Application.Common;
using MediatR;

namespace Application.Commands.AddBatch
{
    public class AddBatchCommand : IRequest<Result<Guid>>
    {
        public Guid ProductId { get; set; }
        public string Code { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
