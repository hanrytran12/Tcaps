using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Materials.Queries.GetAllMaterialToWatch
{
    public class GetAllMaterialToWatchQueryHandler : IRequestHandler<GetAllMaterialToWatchQuery, List<MaterialToWatchDTO>>
    {
        private readonly IAppDbContext _context;
        public GetAllMaterialToWatchQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MaterialToWatchDTO>> Handle(GetAllMaterialToWatchQuery request, CancellationToken cancellationToken)
        {
            return await _context.Materials.AsNoTracking()
                .Select(m => new MaterialToWatchDTO
                {
                    Id = m.Id,
                    Name = m.Name,
                    Quantity = m.Quantity,
                    Description = m.Description,
                    Unit = m.Unit,
                    Price = m.Price,
                }).ToListAsync();
        }
    }
}
