using Application.Common;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.ConfirmRequestFromQc
{
    public class ConfirmRequestFromQcCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
