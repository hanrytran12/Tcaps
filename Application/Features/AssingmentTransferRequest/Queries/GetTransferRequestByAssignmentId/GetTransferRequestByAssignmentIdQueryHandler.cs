using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Queries.GetTransferRequestByAssignmentId
{
    public class GetTransferRequestByAssignmentIdQueryHandler : IRequestHandler<GetTransferRequestByAssignmentIdQuery, TransferRequestDTO>
    {
        private readonly IAppDbContext _appDbContext;

        public GetTransferRequestByAssignmentIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<TransferRequestDTO> Handle(GetTransferRequestByAssignmentIdQuery request, CancellationToken cancellationToken)
        {
            var materialUse = from mu in _appDbContext.MaterialUse
                              where mu.AssignId == request.AssignmentId
                              join ma in _appDbContext.Materials on mu.MaterialId equals ma.Id
                              select new ReconciliationMaterials
                              {
                                  MaterialId = ma.Id,
                                  MaterialName = ma.Name,
                                  ReconciliationQuantity = mu.ReconciledQuantity,
                              };

            var listMaterial = await materialUse.ToListAsync();

            var query = from tq in _appDbContext.AssignmentTransferRequests
                        where tq.AssignmentId == request.AssignmentId
                        join a in _appDbContext.Assignments on tq.AssignmentId equals a.Id
                        join u in _appDbContext.Users on a.WorkshopId equals u.WorkshopId
                        select new TransferRequestDTO
                        {
                            TransferRequestId = tq.Id,
                            AssignmentId = a.Id,
                            Status = tq.Status,
                            Note = tq.Status,
                            CreatedAt = tq.CreatedAt,
                            CreatedBy = new CreatedBy
                            {
                                QcId = u.Id,
                                QcName = u.FullName,
                            },
                            ReconciliationMaterials = listMaterial
                        };

            return await query.FirstOrDefaultAsync();
        }
    }
}
