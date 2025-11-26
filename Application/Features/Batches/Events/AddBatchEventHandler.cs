using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Features.Batches.Events
{
    public class AddBatchEventHandler : INotificationHandler<AddBatchEvent>
    {
        private readonly INotificationService _notificationService;

        public AddBatchEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public Task Handle(AddBatchEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
