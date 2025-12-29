using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Queries.GetAllReworkRequest
{
    public class GetAllReworkRequestQueryHandler : IRequestHandler<GetAllReworkRequestQuery, List<ReworkRequestDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllReworkRequestQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<ReworkRequestDTO>> Handle(GetAllReworkRequestQuery request, CancellationToken cancellationToken)
        {
            var query = from rr in _appDbContext.ReworkRequests
                        join a in _appDbContext.Assignments on rr.AssignmentId equals a.Id
                        join w in _appDbContext.Workshop on a.WorkshopId equals w.Id
                        join b in _appDbContext.Batches on a.BatchId equals b.Id
                        join u in _appDbContext.Users on rr.QcId equals u.Id
                        select new ReworkRequestDTO
                        {
                            Id = rr.Id,
                            BatchCode = b.Code,
                            QcName = u.FullName,
                            LeadId = b.UserId,
                            WorkshopId = w.Id,
                            WorkshopName = w.Name,
                            AssignmentId = a.Id,
                            DefectiveQuantity = rr.DefectiveQuantity,
                            NoteQc = rr.NoteQc,
                            Status = rr.Status,
                            CreatedAt = rr.CreatedAt,
                            DeliveryDate = rr.DeliveryDate,
                            EndDate = rr.EndDate,
                            NextStepDeliveryDate = rr.NextStepDeliveryDate,
                            RequiresMaterialDelivery = a.RequiresMaterialDelivery
                        };

            return await query.ToListAsync();
        }
    }
}
