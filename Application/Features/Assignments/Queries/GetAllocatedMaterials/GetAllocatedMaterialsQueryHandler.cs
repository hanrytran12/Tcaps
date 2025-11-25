using Application.DTOs.Response;
using Application.Features.Assignments.Queries.GetAllocatedMaterials;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllocatedMaterialsQueryHandler : IRequestHandler<GetAllocatedMaterialsQuery, List<AllocatedMaterialDto>>
{
    private readonly IAppDbContext _context;

    public GetAllocatedMaterialsQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AllocatedMaterialDto>> Handle(GetAllocatedMaterialsQuery request, CancellationToken cancellationToken)
    {
        var assignmentStatus = await _context.Assignments
            .AsNoTracking()
            .Where(a => a.Id == request.AssignmentId)
            .Select(a => a.Status)
            .FirstOrDefaultAsync(cancellationToken);

        if (assignmentStatus is null)
        {
            return new List<AllocatedMaterialDto>();
        }

        var query = _context.MaterialUse.AsNoTracking()
            .Where(mu => mu.AssignId == request.AssignmentId);

        if (assignmentStatus == "InProgress")
        {
            query = query.Where(mu => mu.ReworkRequestId == null);
        }
        else if (assignmentStatus == "Reworking")
        {
            query = query.Where(mu => mu.ReworkRequestId != null);
        }

        var result = await query
            .Join(_context.Materials,
                  mu => mu.MaterialId,
                  ma => ma.Id,
                  (mu, ma) => new AllocatedMaterialDto
                  {
                      MaterialId = ma.Id,
                      MaterialName = ma.Name
                  })
            .Distinct()
            .ToListAsync(cancellationToken);

        return result;
    }
}