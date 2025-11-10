using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Queries.GetAllTransferRequest
{
    public class GetAllTransferRequestQueryHandler : IRequestHandler<GetAllTransferRequestQuery, List<TrasnferRequestDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllTransferRequestQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<TrasnferRequestDTO>> Handle(GetAllTransferRequestQuery request, CancellationToken cancellationToken)
        {
            var query = from tr in _appDbContext.AssignmentTransferRequests
                        join a in _appDbContext.Assignments on tr.AssignmentId equals a.Id
                        join w in _appDbContext.Workshop on a.WorkshopId equals w.Id
                        join b in _appDbContext.Batches on a.BatchId equals b.Id
                        join u in _appDbContext.Users on tr.UserId equals u.Id
                        select new TrasnferRequestDTO
                        {
                            TransferRequestId = tr.Id,
                            UserName = u.FullName,
                            BatchCode = b.Code,
                            WorkshopName = w.Name,
                            CompletedQuantity = tr.CompletedQuantity,
                            Note = tr.Note,
                            Status = tr.Status,
                        };

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
