using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Workshop.Queries.GetWorkshopTemplate
{
    public class GetWorkshopTemplateQueryHandler : IRequestHandler<GetWorkshopTemplateQuery, List<WorkshopsDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetWorkshopTemplateQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<WorkshopsDTO>> Handle(GetWorkshopTemplateQuery request, CancellationToken cancellationToken)
        {
            return await _appDbContext.Workshop
                .OrderBy(w => w.StepOrder)
                .Select(w => new WorkshopsDTO
                {
                    WorkshopId = w.Id,
                    WorkshopName = w.Name,
                    Description = w.Description,
                    StepOrder = w.StepOrder,
                }).ToListAsync();
        }
    }
}
