using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Queries.GetPendingRequestForQc
{
    public class GetPendingRequestForQcQueryHandler : IRequestHandler<GetPendingRequestForQcQuery, List<PendingRequestDTO>>
    {
        private readonly IAppDbContext _appDbContext;
        public GetPendingRequestForQcQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<PendingRequestDTO>> Handle(GetPendingRequestForQcQuery request, CancellationToken cancellationToken)
        {
            var qcUser = await _appDbContext.Users.AsNoTracking().Select(u => new { u.Id, u.WorkshopId }).FirstOrDefaultAsync(u => u.Id == request.QcId);

            if (qcUser == null || qcUser.WorkshopId == Guid.Empty)
            {
                return new List<PendingRequestDTO>();
            }

            var pendingRequests = from mr in _appDbContext.MaterialRequests
                                  join a in _appDbContext.Assignments on mr.AssignId equals a.Id
                                  join m in _appDbContext.Materials on mr.MaterialId equals m.Id
                                  join b in _appDbContext.Batches on mr.BatchId equals b.Id
                                  where a.WorkshopId == qcUser.WorkshopId && mr.Status == "Pending"
                                  select new PendingRequestDTO
                                  {
                                      RequestId = mr.Id,
                                      MaterialName = m.Name,
                                      QuantityRequest = mr.QuantityRequest,
                                      BatchCode = b.Code,
                                  };

            return await pendingRequests.ToListAsync();
        }
    }
}
