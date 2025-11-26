using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Queries.GetReworkForDashboard
{
    public class GetReworkForDashboardQueryHandler : IRequestHandler<GetReworkForDashboardQuery, ReworkRequestDTO>
    {
        private readonly IAppDbContext _appDbContext;

        public GetReworkForDashboardQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ReworkRequestDTO> Handle(GetReworkForDashboardQuery request, CancellationToken cancellationToken)
        {
            var query = from rr in _appDbContext.ReworkRequests
                        where rr.AssignmentId == request.AssignmentId
                        join a in _appDbContext.Assignments on rr.AssignmentId equals a.Id
                        join w in _appDbContext.Workshop on a.WorkshopId equals w.Id
                        join b in _appDbContext.Batches on a.BatchId equals b.Id
                        join u in _appDbContext.Users on rr.QcId equals u.Id
                        select new ReworkRequestDTO
                        {
                            Id = rr.Id,
                            BatchCode = b.Code,
                            QcName = u.FullName,
                            WorkshopName = w.Name,
                            DefectiveQuantity = rr.DefectiveQuantity,
                            NoteQc = rr.NoteQc,
                            Status = rr.Status,
                            CreatedAt = rr.CreatedAt,
                            DeliveryDate = rr.DeliveryDate,
                            EndDate = rr.EndDate,
                            NextStepDeliveryDate = rr.NextStepDeliveryDate,
                            RequiresMaterialDelivery = a.RequiresMaterialDelivery
                        };

            return await query.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
