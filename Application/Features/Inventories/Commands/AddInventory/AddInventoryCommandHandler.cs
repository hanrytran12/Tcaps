using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Inventories.Commands.AddInventory
{
    public class AddInventoryCommandHandler : IRequestHandler<AddInventoryCommand, Result<Guid>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IFileStorageService _fileStorageService;

        public AddInventoryCommandHandler(IInventoryRepository inventoryRepository, IMaterialRepository materialRepository, IFileStorageService fileStorageService)
        {
            _inventoryRepository = inventoryRepository;
            _materialRepository = materialRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<Guid>> Handle(AddInventoryCommand request, CancellationToken cancellationToken)
        {
            string imageUrl = await _fileStorageService.SaveFileAsync(request.ImageURL, "inventories", cancellationToken);

            var materials = await _materialRepository.GetByIdAsync(request.MaterialId);
            if (materials is null)
            {
                return Result<Guid>.Failure("Material not found.");
            }

            var inventory = Inventory.Create(request.MaterialId, request.Quantity, imageUrl, request.Price);
            await _inventoryRepository.AddAsync(inventory);

            return Result<Guid>.Success(inventory.Id);
        }
    }
}
