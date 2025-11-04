using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Incomes.Command.AddIncome;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.ComponentDefect.Events
{
    public class ComponentConfirmEventHandler : INotificationHandler<ComponentConfirmEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly IMediator _mediator;
        private readonly IProductionRepository _productionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IComponentDefectRepository _componentDefectRepository;

        public ComponentConfirmEventHandler(INotificationService notificationService,
            IMediator mediator,
            IProductionRepository productionRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IEvaluateRepository evaluateRepository,
            IComponentDefectRepository componentDefectRepository)
        {
            _notificationService = notificationService;
            _mediator = mediator;
            _productionRepository = productionRepository;
            _userRepository = userRepository;
            _evaluateRepository = evaluateRepository;
            _componentDefectRepository = componentDefectRepository;
        }
        public async Task Handle(ComponentConfirmEvent notification, CancellationToken cancellationToken)
        {
            await _notificationService.SendComponentConfirmNotification(notification.Id, notification.EvaluateId, notification.Quantity, notification.Status);

            if (notification.Status == "Confirmed")
            {
                var evaluate = await _evaluateRepository.GetByIdAsync(notification.EvaluateId);
                var componentDefects = await _componentDefectRepository.GetAllByEvaluateIdAsync(evaluate.Id);
                var production = await _productionRepository.GetByIdAsync(evaluate.ProductionId);
                var staff = await _userRepository.GetByIdAsync(production.UserId);

                if (componentDefects.All(c => c.Status == "Confirmed"))
                {
                    await _mediator.Send(new AddIncomeCommand
                    {
                        ProductionId = production.Id,
                        UserId = staff.Id
                    });
                }
            }
        }
    }
}
