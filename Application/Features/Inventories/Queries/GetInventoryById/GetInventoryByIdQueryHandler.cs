using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Inventories.Queries.GetInventoryById
{
    public class GetInventoryByIdQueryHandler : IRequestHandler<GetInventoryByIdQuery, InventoryResponseDTO>
    {
        private readonly IInventoryRepository _inventoryRepository;
        public GetInventoryByIdQueryHandler(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<InventoryResponseDTO> Handle(GetInventoryByIdQuery request, CancellationToken cancellationToken)
        {
            var inventory = await _inventoryRepository.GetByIdAsync(request.Id);
            if (inventory is null)
            {
                throw new NotFoundException("Inventory not found.");
            }
            return new InventoryResponseDTO
            {
                Id = inventory.Id,
                MaterialId = inventory.MaterialId,
                Quantity = inventory.Quantity,
                Date = inventory.Date,
                Price = inventory.Price,
                ImageURL = inventory.ImageURL
            };
        }
    }
}
