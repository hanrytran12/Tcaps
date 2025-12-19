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

        public InsertWorkshopCommandHandler(IWorkshopRepository workshopRepository)
        {
            _workshopRepository = workshopRepository;
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

            if (workshop.StepOrder != null)
            {
                throw new BadRequestException("Xưởng này đã được phân StepOrder");
            }

            int newStepOrder;
            var maxStep = await _workshopRepository.GetMaxStepOrderAsync();
            if (request.PreviousWorkshopId == null)
            {
                newStepOrder = 1;
                await _workshopRepository.ShiftStepOrdersAsync(newStepOrder);
            }
            else
            {
                var afterWorkshop = await _workshopRepository
                .GetByIdAsync(request.PreviousWorkshopId.Value)
                ?? throw new NotFoundException("Xưởng chèn sau không tồn tại.");

                if (afterWorkshop.StepOrder == null)
                    throw new BadRequestException("Xưởng chèn sau chưa nằm trong flow.");

                newStepOrder = afterWorkshop.StepOrder.Value + 1;

                await _workshopRepository.ShiftStepOrdersAsync(newStepOrder);
            }

            workshop.Insert(newStepOrder);
            return Result<Guid>.Success(workshop.Id);
        }
    }
}
