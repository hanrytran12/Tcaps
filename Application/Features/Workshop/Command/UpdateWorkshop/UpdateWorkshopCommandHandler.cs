using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Workshop.Command.UpdateWorkshop
{
    public class UpdateWorkshopCommandHandler : IRequestHandler<UpdateWorkshopCommand, Result<Guid>>
    {
        private readonly IWorkshopRepository _workshopRepository;

        public UpdateWorkshopCommandHandler(IWorkshopRepository workshopRepository)
        {
            _workshopRepository = workshopRepository;
        }
        public async Task<Result<Guid>> Handle(UpdateWorkshopCommand request, CancellationToken cancellationToken)
        {
            var workshop = await _workshopRepository.GetByIdAsync(request.WorkshopId);
            if (workshop is null)
            {
                throw new NotFoundException("Không tìm thấy xưởng.");
            }

            var existsName = await _workshopRepository.ExistNameAsync(request.Name);
            if (existsName)
            {
                throw new BadRequestException("Tên xưởng này đã tồn tại.");
            }

            workshop.Update(request.Name, request.Description);
            return Result<Guid>.Success(workshop.Id);
        }
    }
}
