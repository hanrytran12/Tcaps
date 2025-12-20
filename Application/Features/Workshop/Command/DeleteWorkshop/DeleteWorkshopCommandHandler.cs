using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Workshop.Command.DeleteWorkshop
{
    public class DeleteWorkshopCommandHandler : IRequestHandler<DeleteWorkshopCommand, Result<Guid>>
    {
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public DeleteWorkshopCommandHandler(IWorkshopRepository workshopRepository, IAssignmentRepository assignmentRepository)
        {
            _workshopRepository = workshopRepository;
            _assignmentRepository = assignmentRepository;
        }
        public async Task<Result<Guid>> Handle(DeleteWorkshopCommand request, CancellationToken cancellationToken)
        {
            var workshop = await _workshopRepository.GetByIdAsync(request.WorkshopId);
            if (workshop is null)
            {
                throw new NotFoundException("Xưởng này không tồn tại.");
            }

            if (workshop.Status == "Assigned")
            {
                throw new BadRequestException("Xưởng đã tham gia sản xuất không nên xóa.");
            }

            if (workshop.StepOrder.HasValue)
            {
                int oldStep = workshop.StepOrder.Value;

                workshop.RemoveFromFlow();

                int? maxStep = await _workshopRepository.GetMaxStepOrderAsync();

                await _workshopRepository.ShiftStepOrdersUpAsync(
                    from: oldStep + 1,
                    to: maxStep.Value
                );
            }

            _workshopRepository.Delete(workshop);
            return Result<Guid>.Success(workshop.Id);
        }
    }
}
