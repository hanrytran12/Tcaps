using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Batches.Commands.AddBatch
{
    public class AddBatchCommand : IRequest<Result<Guid>>
    {
        public string CodeProduct { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public IFormFile? ImageFile { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
