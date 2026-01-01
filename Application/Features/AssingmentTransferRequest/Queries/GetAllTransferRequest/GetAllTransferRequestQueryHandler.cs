using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Queries.GetAllTransferRequest
{
    public class GetAllTransferRequestQueryHandler : IRequestHandler<GetAllTransferRequestQuery, List<AssignmentTransferRequestDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllTransferRequestQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<AssignmentTransferRequestDTO>> Handle(GetAllTransferRequestQuery request, CancellationToken cancellationToken)
        {
            var query = from tr in _appDbContext.AssignmentTransferRequests.AsNoTracking()
                        join a in _appDbContext.Assignments.AsNoTracking() on tr.AssignmentId equals a.Id
                        join w in _appDbContext.Workshop.AsNoTracking() on a.WorkshopId equals w.Id
                        join b in _appDbContext.Batches.AsNoTracking() on a.BatchId equals b.Id
                        join p in _appDbContext.Products.AsNoTracking() on b.ProductId equals p.Id
                        join u in _appDbContext.Users.AsNoTracking() on tr.UserId equals u.Id
                        where b.UserId == request.LeadId
                        select new AssignmentTransferRequestDTO
                        {
                            TransferRequestId = tr.Id,
                            AssignmentId = tr.AssignmentId,
                            ReworkRequestId = tr.ReworkRequestId ?? null,
                            BatchId = a.BatchId,
                            WorkshopId = a.WorkshopId,
                            UserName = u.FullName,
                            BatchCode = b.Code,
                            ProductCode = p.Code,
                            ProductName = p.Name,
                            WorkshopName = w.Name,
                            CompletedQuantitySend = tr.CompletedQuantitySend,
                            CompletedQuantityReceive = tr.CompletedQuantityReceive,
                            Note = tr.Note ?? string.Empty,
                            NoteLead = tr.NoteLead ?? string.Empty,
                            Status = tr.Status,
                            CreatedAt = tr.CreatedAt,
                            NextWorkshopName = _appDbContext.Assignments
                                .Where(nextA => nextA.BatchId == a.BatchId && nextA.StepOrder > a.StepOrder)
                                .OrderBy(nextA => nextA.StepOrder)
                                .Join(_appDbContext.Workshop,
                                      nextA => nextA.WorkshopId,
                                      nextW => nextW.Id,
                                      (nextA, nextW) => nextW.Name)
                                .FirstOrDefault() ?? "QC Gác Cổng"
                        };

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
