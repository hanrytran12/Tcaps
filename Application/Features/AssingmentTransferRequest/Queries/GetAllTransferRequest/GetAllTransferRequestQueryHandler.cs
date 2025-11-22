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
            var query = from tr in _appDbContext.AssignmentTransferRequests.AsNoTracking()
                        join a in _appDbContext.Assignments.AsNoTracking() on tr.AssignmentId equals a.Id
                        join w in _appDbContext.Workshop.AsNoTracking() on a.WorkshopId equals w.Id
                        join b in _appDbContext.Batches.AsNoTracking() on a.BatchId equals b.Id
                        join p in _appDbContext.Products.AsNoTracking() on b.ProductId equals p.Id
                        join u in _appDbContext.Users.AsNoTracking() on tr.UserId equals u.Id
                        select new TrasnferRequestDTO
                        {
                            TransferRequestId = tr.Id,
                            UserName = u.FullName,
                            BatchCode = b.Code,
                            ProductCode = p.Code,
                            WorkshopName = w.Name,
                            CompletedQuantity = tr.CompletedQuantity,
                            Note = tr.Note,
                            Status = tr.Status,
                        };

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
