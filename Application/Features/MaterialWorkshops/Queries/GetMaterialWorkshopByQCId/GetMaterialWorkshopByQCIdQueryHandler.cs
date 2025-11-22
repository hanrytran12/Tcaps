using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialWorkshops.Queries.GetMaterialWorkshopByQCId
{
    public class GetMaterialWorkshopByQCIdQueryHandler : IRequestHandler<GetMaterialWorkshopByQCIdQuery, List<MaterialWorkshopDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetMaterialWorkshopByQCIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<MaterialWorkshopDTO>> Handle(GetMaterialWorkshopByQCIdQuery request, CancellationToken cancellationToken)
        {
            var query = from mw in _appDbContext.MaterialWorkshops
                        where mw.WorkshopId == request.WorkshopId && mw.Status == "Pending"
                        join a in _appDbContext.Assignments on mw.AssignId equals a.Id
                        join w in _appDbContext.Workshop on a.WorkshopId equals w.Id
                        join b in _appDbContext.Batches on a.BatchId equals b.Id
                        join p in _appDbContext.Products on b.ProductId equals p.Id
                        select new MaterialWorkshopDTO
                        {
                            Id = mw.Id,
                            WorkshopId = mw.WorkshopId,
                            WorkshopName = w.Name,
                            BatchCode = b.Code,
                            ProductCode = p.Code,
                            AssignId = a.Id,
                            QuantitySend = mw.QuantitySend,
                            QuantityReceive = mw.QuantityReceive,
                            ShipDate = mw.ShipDate,
                            CreatedAt = mw.CreatedAt,
                        };
            return await query.ToListAsync(cancellationToken);
        }
    }
}
