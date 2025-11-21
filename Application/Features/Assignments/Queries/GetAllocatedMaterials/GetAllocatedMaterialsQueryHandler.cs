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
            var assignment = await _context.Assignments.AsNoTracking()
                                .FirstOrDefaultAsync(a => a.Id == request.AssignmentId);

            if (assignment == null)
            {
                // Xử lý trường hợp không tìm thấy Assignment
                return new List<AllocatedMaterialDto>();
            }
            var materialUse = from mu in _context.MaterialUse
                              join ma in _context.Materials on mu.MaterialId equals ma.Id
                              where mu.AssignId == request.AssignmentId
                              select new
                              {
                                  mu.MaterialId,
                                  MaterialName = ma.Name,
                                  mu.ReworkRequestId
                              };

            var filteredQuery = materialUse;

            if (assignment.Status == "InProgress")
            {
                filteredQuery = filteredQuery.Where(x => x.ReworkRequestId == null);
            }
            else if (assignment.Status == "Reworking")
            {
                filteredQuery = filteredQuery.Where(x => x.ReworkRequestId != null);
            }

            var result = await filteredQuery
                .GroupBy(x => x.MaterialId)
                .Select(g => g.First())
                .Select(x => new AllocatedMaterialDto
                {
                    MaterialId = x.MaterialId,
                    MaterialName = x.MaterialName,
                })
                .ToListAsync();

            return result;
        }
    }
}
