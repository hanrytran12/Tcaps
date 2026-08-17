using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssignmentTransferRequest.Queries.GetTransferRequestByAssignmentId
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
            var query = from tq in _appDbContext.AssignmentTransferRequests
                        where tq.AssignmentId == request.AssignmentId && tq.Status == "PendingApproval"
                        join u in _appDbContext.Users on tq.UserId equals u.Id

                        select new
                        {
                            TransferRequest = tq,
                            QcUser = u
                        };

            var resultInfo = await query.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            if (resultInfo == null)
            {
                return null;
            }

            var transferRequest = resultInfo.TransferRequest;
            var qcUser = resultInfo.QcUser;
            var listMaterial = new List<ReconciliationMaterials>();

            var materialQuery = from mu in _appDbContext.MaterialUses
                                join ma in _appDbContext.Materials on mu.MaterialId equals ma.Id
                                select new { mu, ma };

            if (transferRequest.ReworkRequestId.HasValue)
            {
                listMaterial = await materialQuery
                    .Where(x => x.mu.ReworkRequestId == transferRequest.ReworkRequestId.Value)
                    .Select(x => new ReconciliationMaterials
                    {
                        MaterialId = x.ma.Id,
                        MaterialName = x.ma.Name,
                        ReconciliationQuantity = x.mu.ReconciledQuantity
                    })
                    .ToListAsync(cancellationToken);
            }
            else
            {
                listMaterial = await materialQuery
                    .Where(x => x.mu.AssignId == request.AssignmentId && x.mu.ReworkRequestId == null)
                    .Select(x => new ReconciliationMaterials
                    {
                        MaterialId = x.ma.Id,
                        MaterialName = x.ma.Name,
                        ReconciliationQuantity = x.mu.ReconciledQuantity
                    })
                    .ToListAsync(cancellationToken);
            }

            var dto = new TransferRequestDTO
            {
                TransferRequestId = transferRequest.Id,
                AssignmentId = transferRequest.AssignmentId,
                Status = transferRequest.Status,
                Note = transferRequest.Note,
                CreatedAt = transferRequest.CreatedAt,
                CreatedBy = new CreatedBy
                {
                    QcId = qcUser.Id,
                    QcName = qcUser.FullName,
                },
                ReconciliationMaterials = listMaterial
            };

            return dto;
        }
    }
}