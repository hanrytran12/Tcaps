using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Inventories.Commands.AddInventory
{
    public class AddInventoryCommandHandler : IRequestHandler<AddInventoryCommand, Result<Guid>>
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMaterialRepository _materialRepository;
        public AddInventoryCommandHandler(IInventoryRepository inventoryRepository, IUnitOfWork unitOfWork, IMaterialRepository materialRepository)
        {
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
            _materialRepository = materialRepository;
        }

        public async Task<Result<Guid>> Handle(AddInventoryCommand request, CancellationToken cancellationToken)
        {
            string imageUrl = string.Empty;
            if (request.ImageURL != null && request.ImageURL.Length > 0)
            {
                string uploadPath = Path.Combine("wwwroot", "images", "inventories");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + request.ImageURL.FileName;
                string filePath = Path.Combine(uploadPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageURL.CopyToAsync(stream, cancellationToken);
                }
                imageUrl = $"/images/inventories/{uniqueFileName}";
            }

            var materials = await _materialRepository.GetByIdAsync(request.MaterialId);
            if (materials is null)
            {
                return Result<Guid>.Failure("Material not found.");
            }

            var inventory = Inventory.Create(request.MaterialId, request.Quantity, imageUrl);
            await _inventoryRepository.AddAsync(inventory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(inventory.Id);
        }
    }
}
