using Application.Common;
using MediatR;

namespace Application.Features.MaterialRequest.Commands.UpdateMaterialRequest
{
    public class UpdateMaterialRequestCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
