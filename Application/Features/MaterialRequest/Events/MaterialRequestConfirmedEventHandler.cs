using Domain.Events;
using MediatR;

namespace Application.Features.MaterialRequest.Events
{
    public class MaterialRequestConfirmedEventHandler : INotificationHandler<MaterialRequestConfirmedEvent>
    {
        public Task Handle(MaterialRequestConfirmedEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
