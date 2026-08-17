using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialUses.Query.GetMaterialUseByAssignId
{
    public class GetMaterialUseByAssignIdQueryHandler : IRequestHandler<GetMaterialUseByAssignIdQuery, List<MaterialUseDTO>>
    {
        private readonly IAppDbContext _context;

        public GetMaterialUseByAssignIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<MaterialUseDTO>> Handle(GetMaterialUseByAssignIdQuery request, CancellationToken cancellationToken)
        {
            var query = from mu in _context.MaterialUses
                        join m in _context.Materials on mu.MaterialId equals m.Id
                        where mu.AssignId == request.AssignId
                        select new MaterialUseDTO
                        {
                            Id = mu.Id,
                            MaterialId = mu.MaterialId,
                            ReworkRequestId = mu.ReworkRequestId,
                            MaterialName = m.Name,
                            QuantityDivide = mu.QuantityDivide,
                            QuantityStaffUse = mu.QuantityStaffUse,
                            ReconciledQuantity = mu.ReconciledQuantity,
                            QuantityRequest = mu.QuantityRequest,
                            Date = mu.Date
                        };

            if (!string.IsNullOrWhiteSpace(request.MaterialName))
            {
                query = query.Where(x => x.MaterialName.Contains(request.MaterialName));
            }

            var resultList = await query
                .OrderByDescending(x => x.MaterialName)
                .ToListAsync();

            return resultList;
        }
    }
}
