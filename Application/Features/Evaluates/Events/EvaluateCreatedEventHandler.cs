using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Incomes.Command.AddIncome;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Evaluates.Events
{
    public class EvaluateCreatedEventHandler : INotificationHandler<EvaluateCreatedEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IProductionRepository _productionRepository;
        private readonly IUserRepository _userRepository;

        public EvaluateCreatedEventHandler(INotificationService notificationService, IUnitOfWork unitOfWork, 
            IMediator mediator, IProductionRepository productionRepository, IUserRepository userRepository)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _productionRepository = productionRepository;
            _userRepository = userRepository;
        }
        public async Task Handle(EvaluateCreatedEvent notification, CancellationToken cancellationToken)
        {
            var production = await _productionRepository.GetByIdAsync(notification.ProductionId);
            var staff = await _userRepository.GetByIdAsync(production.UserId);

            if (notification.Status == "Passed" || notification.Status == "Rejected")
            {
                await _mediator.Send(new AddIncomeCommand
                {
                    ProductionId = production.Id,
                    UserId = staff.Id
                });
            }
            await _notificationService.SendEvaluateFixErrorNotificationAsync(notification.EvaluateId, notification.ProductionId, notification.UserId.Value, notification.QuantityError, notification.Note, notification.Status);
            //await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
