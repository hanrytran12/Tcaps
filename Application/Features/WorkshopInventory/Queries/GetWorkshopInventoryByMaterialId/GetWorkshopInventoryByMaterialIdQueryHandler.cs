using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByMaterialId
{
    public class GetWorkshopInventoryByMaterialIdQueryHandler : IRequestHandler<GetWorkshopInventoryByMaterialIdQuery, Domain.Entities.WorkshopInventory>
    {
        private readonly IWorkshopInventoryRepository _repository;

        public GetWorkshopInventoryByMaterialIdQueryHandler(IWorkshopInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Domain.Entities.WorkshopInventory> Handle(GetWorkshopInventoryByMaterialIdQuery request, CancellationToken cancellationToken)
        {
            var workshopInventory = await _repository.GetByMaterialIdAsync(request.MaterialId);
            if (workshopInventory is null)
            {
                throw new NotFoundException("Workshop inventory not found.");
            }

            return workshopInventory;
        }
    }
}
