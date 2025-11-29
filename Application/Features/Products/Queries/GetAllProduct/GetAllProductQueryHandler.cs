using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.Queries.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<ProductsDTO>>
    {
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public GetAllProductQueryHandler(IAppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<List<ProductsDTO>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .AsNoTracking()
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProductsDTO
                {
                    ProductId = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    Image = _fileStorageService.GetFileUrl(p.Image),
                    Description = p.Description,
                    CreatedAt = p.CreatedAt
                });

            return await query.ToListAsync();
        }
    }
}
