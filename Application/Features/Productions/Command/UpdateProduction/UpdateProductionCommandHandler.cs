using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Productions.Command.UpdateProduction
{
    public class UpdateProductionCommandHandler : IRequestHandler<UpdateProductionCommand, Result<Guid>>
    {
        private readonly IProductionRepository _productionRepository;

        public UpdateProductionCommandHandler(IProductionRepository productionRepository)
        {
            _productionRepository = productionRepository;
        }
        public async Task<Result<Guid>> Handle(UpdateProductionCommand request, CancellationToken cancellationToken)
        {
            var production = await _productionRepository.GetByIdAsync(request.ProductionId);
            if (production == null)
            {
                throw new NotFoundException("Production not found.");
            }

            production.SetQuantity(request.Quantity);
            _productionRepository.Update(production);

            return Result<Guid>.Success(production.Id);
        }
    }
}
