using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Queries.GetAllocatedMaterials
{
    public class GetAllocatedMaterialsQueryHandler : IRequestHandler<GetAllocatedMaterialsQuery, List<AllocatedMaterialDto>>
    {
        private readonly IAppDbContext _context;
        public GetAllocatedMaterialsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AllocatedMaterialDto>> Handle(GetAllocatedMaterialsQuery request, CancellationToken cancellationToken)
        {
            var materialUse = from mu in _context.MaterialUse
                              join ma in _context.Materials on mu.MaterialId equals ma.Id
                              where mu.AssignId == request.AssignmentId
                              select new AllocatedMaterialDto
                              {
                                  MaterialId = mu.MaterialId,
                                  MaterialName = ma.Name,
                              };

            return await materialUse.AsNoTracking().ToListAsync();
        }
    }
}
