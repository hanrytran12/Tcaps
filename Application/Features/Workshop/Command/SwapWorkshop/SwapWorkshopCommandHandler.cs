using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Workshop.Command.SwapWorkshop
{
    public class SwapWorkshopCommandHandler : IRequestHandler<SwapWorkshopCommand, Result<string>>
    {
        private readonly IWorkshopRepository _workshopRepository;

        public SwapWorkshopCommandHandler(IWorkshopRepository workshopRepository)
        {
            _workshopRepository = workshopRepository;
        }
        public async Task<Result<string>> Handle(SwapWorkshopCommand request, CancellationToken cancellationToken)
        {
            var w1 = await _workshopRepository.GetByIdAsync(request.WorkshopId1);
            var w2 = await _workshopRepository.GetByIdAsync(request.WorkshopId2);

            if (w1 is null || w2 is null)
            {
                throw new NotFoundException("Xưởng không tồn tại.");
            }

            if (w1.WorkshopType == WorkshopType.Outsource ||
                w2.WorkshopType == WorkshopType.Outsource)
            {
                throw new BadRequestException("Không thể đổi xưởng khoán");
            }

            w1.SwapStepOrder(w2);
            return Result<string>.Success("Đổi thành công");
        }
    }
}
