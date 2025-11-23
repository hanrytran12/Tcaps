using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Workshop.Command.AddWorkshop
{
    public class AddWorkshopCommandHandler : IRequestHandler<AddWorkshopCommand, Result<Guid>>
    {
        private readonly IWorkshopRepository _workshopRepository;

        public AddWorkshopCommandHandler(IWorkshopRepository workshopRepository)
        {
            _workshopRepository = workshopRepository;
        }
        public async Task<Result<Guid>> Handle(AddWorkshopCommand request, CancellationToken cancellationToken)
        {
            bool isNameExists = await _workshopRepository.ExistNameAsync(request.Name);
            bool isStepOrderExists = await _workshopRepository.ExistsStepOrderAsync(request.StepOrder);

            if (isNameExists)
            {
                return Result<Guid>.Failure($"Workshop with name '{request.Name}' already exists.");
            }

            if (isStepOrderExists)
            {
                return Result<Guid>.Failure($"Workshop with step order '{request.StepOrder}' already exists.");
            }

            var workshop = Domain.Entities.Workshop.Create(request.Name, request.Description, request.StepOrder);
            await _workshopRepository.AddAsync(workshop);

            return Result<Guid>.Success(workshop.Id);
        }
    }
}
