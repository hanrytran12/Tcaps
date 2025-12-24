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
            var productions = await (from pro in _context.Productions
                                     join u in _context.Users on pro.UserId equals u.Id
                                     where pro.AssignId == request.AssignId
                                     select new ProductionDTO
                                     {
                                         Id = pro.Id,
                                         AssignId = request.AssignId,
                                         UserId = u.Id,
                                         FullName = u.FullName,
                                         Quantity = pro.Quantity,
                                         Date = pro.Date,
                                         Time = pro.Time,
                                         Status = pro.Status
                                     })
                                     .ToListAsync();

            return productions;
        }
    }
}
