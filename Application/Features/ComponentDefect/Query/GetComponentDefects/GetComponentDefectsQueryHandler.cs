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
                        where evaluate.UserId == Guid.Parse("E5F6A7B8-C9D1-4E5F-3A4B-5C6D7E8F9A1B")
                        select new ComponentDefectsDTO
                        {
                            DefectType = defect.DefectType,
                            Serverity = defect.Serverity,
                            Description = defect.Description,
                            NameStaff = staff.FullName,
                            Status = defect.Status
                        };

            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(query => query.Status == request.Status);
            }

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
