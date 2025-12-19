using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Queries.GetForAssignmentDashboard
{
    public class GetForAssignmentDashboardQueryHandler : IRequestHandler<GetForAssignmentDashboardQuery, List<MaterialRequestForAssignmentDashboardDTO>>
    {
        private readonly IAppDbContext _context;

        public GetForAssignmentDashboardQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<MaterialRequestForAssignmentDashboardDTO>> Handle(GetForAssignmentDashboardQuery request, CancellationToken cancellationToken)
        {
            var dtos = await (
                from m in _context.MaterialRequests
                join material in _context.Materials on m.MaterialId equals material.Id
                where m.AssignId == request.AssignmentId
                group new { m, material } by new { m.MaterialId, material.Name, material.Unit } into g
                select new MaterialRequestForAssignmentDashboardDTO
                {
                    MaterialId = g.Key.MaterialId,
                    MaterialName = g.Key.Name,
                    QuantityRequest = g.Sum(x => x.m.QuantityRequest),
                    QuantityResponse = g.Sum(x => x.m.ActualReceivedQuantity),
                    QuantityActualAndStock = g.Sum(
                        x => x.m.ActualReceivedQuantity + x.m.QuantityFromStock
                    ),
                    Unit = g.Key.Unit
                }
            ).ToListAsync(cancellationToken);

            return dtos;
        }
    }
}
