using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Productions.Command.UpdateProduction
{
    public class UpdateProductionCommandHandler : IRequestHandler<UpdateProductionCommand, Result<Guid>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IMediator _mediator;
        private readonly IAppDbContext _context;

        public UpdateProductionCommandHandler(IProductionRepository productionRepository, IMediator mediator, IAppDbContext context)
        {
            _productionRepository = productionRepository;
            _mediator = mediator;
            _context = context;
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

            var batchCode = await (from p in _context.Productions
                        join a in _context.Assignments on p.AssignId equals a.Id
                        join b in _context.Batches on a.BatchId equals b.Id
                        select b.Code).FirstOrDefaultAsync();


            await _mediator.Publish(new UpdateQuantityProductionEvent
            (
                production.UserId, 
                request.Quantity, 
                production.Date, 
                production.Time, 
                batchCode
            ));

            return Result<Guid>.Success(production.Id);
        }
    }
}
