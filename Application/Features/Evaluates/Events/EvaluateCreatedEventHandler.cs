using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Evaluates.Events
{
    public class EvaluateCreatedEventHandler : INotificationHandler<EvaluateCreatedEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly IProductionRepository _productionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EvaluateCreatedEventHandler(INotificationService notificationService, IProductionRepository productionRepository, IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _productionRepository = productionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(EvaluateCreatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Status == "Fail")
            {
                var production = await _productionRepository.GetByIdAsync(notification.ProductionId);
                production.Rework();
                _productionRepository.Update(production);
            }
            await _notificationService.SendEvaluateFixErrorNotificationAsync(notification.EvaluateId, notification.ProductionId, notification.UserId.Value, notification.QuantityError, notification.Note, notification.Status);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
