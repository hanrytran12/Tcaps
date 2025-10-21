using Application.Common;
using MediatR;

namespace Application.Features.Batches.Commands.DeleteBatch
{
    public class DeleteBatchCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public DeleteBatchCommand(Guid id)
        {
            Id = id;
        }
    }
}
