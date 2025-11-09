using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.ReworkRequest.Commands.CreateReworkRequest
{
    public class CreateReworkRequestCommandHandler : IRequestHandler<CreateReworkRequestCommand, Result<Guid>>
    {
        private readonly IReworkRequestRepository _repository;

        public CreateReworkRequestCommandHandler(IReworkRequestRepository reworkRequestRepository)
        {
            _repository = reworkRequestRepository;
        }

        public async Task<Result<Guid>> Handle(CreateReworkRequestCommand request, CancellationToken cancellationToken)
        {
            var reworkRequest = Domain.Entities.ReworkRequest.Create(request.QCId, request.AssigmentId, request.DefectiveQuantity, request.NoteQc);
            await _repository.AddReworkRequestAsync(reworkRequest);
            return Result<Guid>.Success(reworkRequest.Id);
        }
    }
}
