using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ComponentDefect.Query.GetComponentByEvaluateId
{
    public class GetComponentByEvaluatedIdQueryHandler : IRequestHandler<GetComponentByEvaluatedIdQuery, List<ComponentDefectsDTO>>
    {
        private readonly IAppDbContext _context;

        public GetComponentByEvaluatedIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ComponentDefectsDTO>> Handle(GetComponentByEvaluatedIdQuery request, CancellationToken cancellationToken)
        {
            var components = from defect in _context.ComponentDefects
                             join evaluate in _context.Evaluates on defect.EvaluateId equals evaluate.Id
                             join production in _context.Productions on evaluate.ProductionId equals production.Id
                             join staff in _context.Users on production.UserId equals staff.Id
                             where defect.EvaluateId == request.EvaluatedId
                             select new { defect, evaluate, staff };

            var result = components.Select(c => new ComponentDefectsDTO
            {
                Id = c.defect.Id,
                Description = c.defect.Description,
                Quantity = c.defect.Quantity,
                QuantityReject = c.defect.QuantityReject,
                NameStaff = c.staff.FullName,
                Status = c.defect.Status,
            });

            return await result.ToListAsync();
        }
    }
}
