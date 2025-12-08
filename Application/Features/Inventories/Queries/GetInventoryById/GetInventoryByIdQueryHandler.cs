using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Inventories.Queries.GetInventoryById
{
    public class GetInventoryByIdQueryHandler : IRequestHandler<GetInventoryByIdQuery, Inventory>
    {
        private readonly IInventoryRepository _inventoryRepository;
        public GetInventoryByIdQueryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Inventory> Handle(GetInventoryByIdQuery request, CancellationToken cancellationToken)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(request.Id);
            if (inventory is null)
            {
                throw new NotFoundException("Inventory not found.");
            }
            return inventory;
        }
    }
}
