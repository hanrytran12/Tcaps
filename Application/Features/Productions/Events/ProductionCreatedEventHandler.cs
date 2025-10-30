using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Productions.Events
{
    public class ProductionCreatedEventHandler : INotificationHandler<ProductionCreatedEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMaterialUseRepository _materialUseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductionCreatedEventHandler(INotificationService notificationService, 
            IAssignmentRepository assignmentRepository,
            IMaterialUseRepository materialUseRepository,
            IUnitOfWork unitOfWork)
        {
            _notificationService = notificationService;
            _assignmentRepository = assignmentRepository;
            _materialUseRepository = materialUseRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(ProductionCreatedEvent notification, CancellationToken cancellationToken)
        {
            var assign = await _assignmentRepository.GetByIdAsync(notification.AssignId);
            if (assign == null)
                throw new Exception($"Assignment {notification.AssignId} not found.");

            var materialUses = await _materialUseRepository.GetByAssignIdAsync(assign.Id);
            if (materialUses == null)
                throw new Exception($"MaterialUse for Assign {assign.Id} not found.");

            foreach (var item in materialUses)
            {
                item.IncreaseQuantityStaffUse(notification.Quantity);

                decimal maxAllow = item.QuantityDivide + item.QuantityRequest;

                if (item.QuantityDivide == item.QuantityStaffUse)
                {
                    throw new Exception("Đã sử dụng hết vật liệu được phân công.");
                }
                else if (item.QuantityStaffUse >= maxAllow)
                {
                    throw new Exception("Đã sử dụng hết vật liệu (bao gồm cả phần yêu cầu thêm).");
                }

                _materialUseRepository.Update(item);
            }

            await _notificationService.SendSubmitProductionNotification(notification.AssignId, notification.StaffId, notification.Quantity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
