using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Inventories.Queries.GetInventoryByMaterialId
{
    public class GetInventoryByMaterialIdQueryHandler : IRequestHandler<GetInventoryByMaterialIdQuery, InventoryHistoryDTO>
    {
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public GetInventoryByMaterialIdQueryHandler(IAppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<InventoryHistoryDTO> Handle(GetInventoryByMaterialIdQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Inventories
                .Where(i => i.MaterialId == request.MaterialId &&
                            i.Date.Month == request.Month &&
                            i.Date.Year == request.Year)
                .Select(i => new InventoryDTO
                {
                    Date = i.Date,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Image = _fileStorageService.GetFileUrl(i.ImageURL)
                });

            var inventoryList = await query.ToListAsync(cancellationToken);
            return new InventoryHistoryDTO
            {
                TotalPrice = inventoryList.Sum(i => i.Price * i.Quantity),
                TotalQuantity = inventoryList.Sum(i => i.Quantity),
                Inventories = inventoryList
            };
        }
    }
}
