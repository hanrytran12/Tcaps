using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.FinalTransferRequest.Events
{
    public class FinalTransferRequestCreateEventHandler : INotificationHandler<FinalTransferRequestCreatedEvent>
    {
        private readonly IFinalTransferRequestRepository _finalTransferRequestRepository;

        public FinalTransferRequestCreateEventHandler(IFinalTransferRequestRepository finalTransferRequestRepository)
        {
            _finalTransferRequestRepository = finalTransferRequestRepository;
        }
        public async Task Handle(FinalTransferRequestCreatedEvent notification, CancellationToken cancellationToken)
        {
            var finalTransfer = new Domain.Entities.FinalTransferRequest(
                notification.Id,
                notification.AssignTransferRequestId,
                notification.QuantityFinalSend);

            await _finalTransferRequestRepository.AddAsync(finalTransfer);
        }
    }
}
