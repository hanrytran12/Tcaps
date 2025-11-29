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
                                            UserId = mr.UserId,
                                            WorkshopId = u.WorkshopId ?? Guid.Empty,
                                            WorkshopName = wItem.Name ?? string.Empty,
                                            BatchId = mr.BatchId,
                                            AssignId = mr.AssignId,
                                            QuantityRequest = mr.QuantityRequest,
                                            Status = mr.Status,
                                            Note = mr.Note,
                                            Date = mr.Date,
                                            Type = mr.Type,
                                            NoteFromQC = mr.NoteFromQC,
                                            ActualReceivedQuantity = mr.ActualReceivedQuantity
                                        };

            return Result<List<MaterialRequestDTO>>.Success(materialRequestsQuery.ToList());
        }
    }
}
