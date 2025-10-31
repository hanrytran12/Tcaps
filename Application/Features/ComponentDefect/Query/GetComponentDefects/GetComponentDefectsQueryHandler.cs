using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ComponentDefects.Query.GetComponentDefects
{
    public class GetComponentDefectsQueryHandler : IRequestHandler<GetComponentDefectsQuery, List<ComponentDefectsDTO>>
    {
        private readonly IAppDbContext _context;
        public GetComponentDefectsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ComponentDefectsDTO>> Handle(GetComponentDefectsQuery request, CancellationToken cancellationToken)
        {
            var query = from defect in _context.ComponentDefects
                        join evaluate in _context.Evaluates on defect.EvaluateId equals evaluate.Id
                        join production in _context.Productions on evaluate.ProductionId equals production.Id
                        join staff in _context.Users on production.UserId equals staff.Id
                        select new { defect, evaluate, staff };

            query = query.Where(q => q.evaluate.UserId == request.QCId);

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                query = query.Where(q => q.defect.Status == request.Status);
            }

            var finalQuery = query.Select(q => new ComponentDefectsDTO
            {
                DefectType = q.defect.DefectType,
                Serverity = q.defect.Serverity,
                Description = q.defect.Description,
                Solution = q.defect.Solution,
                Quantity = q.defect.Quantity,
                NameStaff = q.staff.FullName,
                Status = q.defect.Status,
            });

            return await finalQuery.AsNoTracking().ToListAsync();
        }
    }
}
