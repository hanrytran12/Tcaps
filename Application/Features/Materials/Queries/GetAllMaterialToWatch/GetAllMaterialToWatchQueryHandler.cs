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
            var query = from ma in _context.Materials.AsNoTracking()
                        join mr in _context.MaterialRequests.AsNoTracking()
                        on ma.Id equals mr.MaterialId into requests

                        select new MaterialToWatchDTO
                        {
                            Id = ma.Id,
                            Name = ma.Name,
                            Description = ma.Description,
                            Quantity = ma.Quantity,
                            Unit = ma.Unit,
                            Price = ma.Price,

                            QuantitySend = (int)requests
                                .Where(r => r.Status == "Confirmed" || r.Status == "ConfirmedWithDiscrepancy")
                                .Sum(r => r.QuantityRequest)
                        };

            return await query.ToListAsync(cancellationToken);
        }
    }
}
