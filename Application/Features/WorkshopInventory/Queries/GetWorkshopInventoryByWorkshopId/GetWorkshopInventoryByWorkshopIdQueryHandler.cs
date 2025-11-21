using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.WorkshopInventory.Queries.GetWorkshopInventoryByWorkshopId
{
    public class GetWorkshopInventoryByWorkshopIdQueryHandler : IRequestHandler<GetWorkshopInventoryByWorkshopIdQuery, List<WorkshopInventoryForExportDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetWorkshopInventoryByWorkshopIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<WorkshopInventoryForExportDTO>> Handle(GetWorkshopInventoryByWorkshopIdQuery request, CancellationToken cancellationToken)
        {
            var query = from w in _appDbContext.Workshop
                        where w.Id == request.WorkshopId
                        join wi in _appDbContext.WorkshopInventory on w.Id equals wi.WorkshopId into workshopInventory
                        from subWi in workshopInventory.DefaultIfEmpty()
                        join m in _appDbContext.Materials on subWi.MaterialId equals m.Id
                        select new WorkshopInventoryForExportDTO
                        {
                            MaterialName = m.Name,
                            Quantity = subWi != null ? subWi.Quantity : 0,
                            Unit = m.Unit
                        };

            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
