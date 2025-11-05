using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Productions.Command.AddProductionReport
{
    public class AddProductionReportCommandHandler : IRequestHandler<AddProductionReportCommand, Result>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IProductionRepository _productionRepository;
        public AddProductionReportCommandHandler(IAppDbContext appDbContext, IProductionRepository productionRepository)
        {
            _appDbContext = appDbContext;
            _productionRepository = productionRepository;
        }

        public async Task<Result> Handle(AddProductionReportCommand request, CancellationToken cancellationToken)
        {
            var production = Production.Create(request.AssignId, request.StaffId, request.Quantity);
            await _productionRepository.AddAsync(production);

            if (request.MaterialUsed.Count() > 0)
            {
                foreach (var items in request.MaterialUsed)
                {
                    var materialUse = await _appDbContext.MaterialUse.FirstOrDefaultAsync(m => m.MaterialId == items.MaterialId && m.AssignId == request.AssignId);

                    if (materialUse is null)
                    {
                        return Result.Failure("Không tìm thấy MaterailUse");
                    }

                    materialUse.IncreaseQuantityStaffUse(items.QuantityUsed);
                }
            }

            return Result.Success();
        }
    }
}
