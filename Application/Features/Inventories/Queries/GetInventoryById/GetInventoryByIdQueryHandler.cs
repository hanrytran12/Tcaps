using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Inventories.Queries.GetInventoryById
{
    public class GetInventoryByIdQueryHandler : IRequestHandler<GetInventoryByIdQuery, Result<Inventory>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        public GetInventoryByIdQueryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Result<Inventory>> Handle(GetInventoryByIdQuery request, CancellationToken cancellationToken)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(request.Id);
            if (inventory is null)
            {
                return Result<Inventory>.Failure("Inventory not found.");
            }
            return Result<Inventory>.Success(inventory);
        }
    }
}
