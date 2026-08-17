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
                        where mw.WorkshopId == request.WorkshopId
                        join a in _appDbContext.Assignments on mw.AssignId equals a.Id
                        join w in _appDbContext.Workshops on a.WorkshopId equals w.Id
                        join b in _appDbContext.Batches on a.BatchId equals b.Id
                        join p in _appDbContext.Products on b.ProductId equals p.Id
                        join u in _appDbContext.Users on mw.SupplierId equals u.Id
                        select new MaterialWorkshopDTO
                        {
                            Id = mw.Id,
                            WorkshopId = mw.WorkshopId,
                            WorkshopName = w.Name,
                            BatchCode = b.Code,
                            ProductCode = p.Code,
                            ProductName = p.Name,
                            SupplierId = mw.SupplierId,
                            SupplierName = u.FullName,
                            AssignId = a.Id,
                            QuantitySend = mw.QuantitySend,
                            QuantityReceive = mw.QuantityReceive,
                            ShipDate = mw.ShipDate,
                            CreatedAt = mw.CreatedAt,
                            Status = mw.Status,
                        };
            return await query.ToListAsync(cancellationToken);
        }
    }
}
