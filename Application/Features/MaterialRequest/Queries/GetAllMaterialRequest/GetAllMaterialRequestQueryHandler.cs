using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Queries.GetAllMaterialRequest
{
    public class GetAllMaterialRequestQueryHandler : IRequestHandler<GetAllMaterialRequestQuery, Result<List<MaterialRequestDTO>>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllMaterialRequestQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Result<List<MaterialRequestDTO>>> Handle(GetAllMaterialRequestQuery request, CancellationToken cancellationToken)
        {
            var materialRequestsQuery = from mr in _appDbContext.MaterialRequests.AsNoTracking()

                                        join a in _appDbContext.Assignments.AsNoTracking()
                                            on mr.AssignId equals a.Id

                                        join b in _appDbContext.Batches.AsNoTracking()
                                            on a.BatchId equals b.Id

                                        join uu in _appDbContext.Users.AsNoTracking()
                                            on b.UserId equals uu.Id

                                        join p in _appDbContext.Products.AsNoTracking()
                                            on b.ProductId equals p.Id

                                        join m in _appDbContext.Materials.AsNoTracking()
                                            on mr.MaterialId equals m.Id

                                        join u in _appDbContext.Users.AsNoTracking()
                                            on mr.UserId equals u.Id

                                        join w in _appDbContext.Workshop.AsNoTracking()
                                            on u.WorkshopId equals w.Id into workshopGroup
                                        from wItem in workshopGroup.DefaultIfEmpty()
                                        select new MaterialRequestDTO
                                        {
                                            Id = mr.Id,
                                            MaterialId = mr.MaterialId,
                                            MaterialName = m.Name,
                                            ProductCode = p.Code,
                                            ProductName = p.Name,
                                            UserId = mr.UserId,
                                            UserName = u.FullName,
                                            UserCreate = uu.FullName,
                                            WorkshopId = u.WorkshopId ?? Guid.Empty,
                                            WorkshopName = wItem.Name ?? string.Empty,
                                            BatchId = mr.BatchId,
                                            BatchCode = b.Code,
                                            AssignId = mr.AssignId,
                                            QuantityRequest = mr.QuantityRequest,
                                            Status = mr.Status,
                                            Note = mr.Note,
                                            Date = mr.Date,
                                            Type = mr.Type,
                                            NoteFromQC = mr.NoteFromQC,
                                            ActualReceivedQuantity = mr.ActualReceivedQuantity,
                                            QuantityFromStock = mr.QuantityFromStock
                                        };

            return Result<List<MaterialRequestDTO>>.Success(materialRequestsQuery.ToList());
        }
    }
}
