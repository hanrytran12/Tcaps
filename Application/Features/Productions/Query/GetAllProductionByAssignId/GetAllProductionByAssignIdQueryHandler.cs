using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Productions.Query.GetAllProductionByAssignId
{
    public class GetAllProductionByAssignIdQueryHandler : IRequestHandler<GetAllProductionByAssignIdQuery, List<ProductionDTO>>
    {
        private readonly IAppDbContext _context;

        public GetAllProductionByAssignIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProductionDTO>> Handle(GetAllProductionByAssignIdQuery request, CancellationToken cancellationToken)
        {
            var userRole = await _context.Users
                .Where(x => x.Id == request.UserId)
                .Select(x => x.Role)
                .FirstOrDefaultAsync();

            var query = from pro in _context.Productions
                        join u in _context.Users on pro.UserId equals u.Id
                        where pro.AssignId == request.AssignId
                        select new
                        {
                            pro.Id,
                            pro.AssignId,
                            pro.ReworkRequestId,
                            UserId = u.Id,
                            u.FullName,
                            pro.QuantitySend,
                            pro.QuantityReceive,
                            pro.Date,
                            pro.Time,
                            pro.Status
                        };

            if (userRole == "Staff")
            {
                query = query.Where(x => x.UserId == request.UserId);
            }

            var productions = await query
                .Select(x => new ProductionDTO
                {
                    Id = x.Id,
                    AssignId = x.AssignId,
                    ReworkRequestId = x.ReworkRequestId,
                    UserId = x.UserId,
                    FullName = x.FullName,
                    QuantitySend = x.QuantitySend,
                    QuantityReceive = x.QuantityReceive,
                    Date = x.Date,
                    Time = x.Time,
                    Status = x.Status
                })
                .ToListAsync(cancellationToken);

            return productions;
        }
    }
}
