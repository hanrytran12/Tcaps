using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Workshop.Command.InsertWorkshop
{
    public class InsertWorkshopCommandHandler : IRequestHandler<InsertWorkshopCommand, Result<Guid>>
    {
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InsertWorkshopCommandHandler(IWorkshopRepository workshopRepository, IUnitOfWork unitOfWork)
        {
            _workshopRepository = workshopRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(InsertWorkshopCommand request, CancellationToken cancellationToken)
        {
            var workshop = await _workshopRepository.GetByIdAsync(request.WorkshopId);
            if (workshop is null)
            {
                throw new NotFoundException("Xưởng này không tồn tại.");
            }

            if (workshop.WorkshopType == Domain.Enums.WorkshopType.Outsource)
            {
                throw new ConflictException("Xưởng khoán không được phép chèn.");
            }

            int targetStep;
            //int? maxStep = await _workshopRepository.GetMaxStepOrderAsync();
            if (request.PreviousWorkshopId == null)
            {
                targetStep = 1;
            }
            else
            {
                var previousWorkshop = await _workshopRepository
                .GetByIdAsync(request.PreviousWorkshopId.Value)
                ?? throw new NotFoundException("Xưởng chèn sau không tồn tại.");

                if (previousWorkshop.StepOrder == null)
                    throw new BadRequestException("Xưởng chèn sau chưa nằm trong flow.");

                int previousStep = previousWorkshop.StepOrder.Value;

                if (workshop.StepOrder.HasValue && workshop.StepOrder.Value < previousStep)
                {
                    targetStep = previousStep;
                }
                else
                {
                    targetStep = previousStep + 1;
                }
            }

            if (workshop.StepOrder == targetStep) return Result<Guid>.Success(workshop.Id);

            int? oldStep = workshop.StepOrder;

            if (oldStep.HasValue)
            {
                workshop.RemoveFromFlow();
                await _workshopRepository.ShiftStepOrdersUpAsync(oldStep.Value + 1, int.MaxValue);
            }

            await _workshopRepository.ShiftStepOrdersDownAsync(targetStep, int.MaxValue);

            _unitOfWork.ClearTracker();

            var workshopToUpdate = await _workshopRepository.GetByIdAsync(request.WorkshopId);
            workshopToUpdate.Insert(targetStep);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(workshopToUpdate.Id);
        }
    }
}
