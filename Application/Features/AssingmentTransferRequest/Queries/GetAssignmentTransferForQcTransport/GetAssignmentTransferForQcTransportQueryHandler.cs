using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Queries.GetAssignmentTransferForQcTransport
{
    public class GetAssignmentTransferForQcTransportQueryHandler : IRequestHandler<GetAssignmentTransferForQcTransportQuery, AssignmentTransferRequestDTO>
    {
        private readonly IAppDbContext _context;

        public GetAssignmentTransferForQcTransportQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<AssignmentTransferRequestDTO> Handle(GetAssignmentTransferForQcTransportQuery request, CancellationToken cancellationToken)
        {
            var dto = from assignTransfer in _context.AssignmentTransferRequests
                      join assignment in _context.Assignments on assignTransfer.AssignmentId equals assignment.Id
                      join batch in _context.Batches on assignment.BatchId equals batch.Id
                      join product in _context.Products on batch.ProductId equals product.Id
                      join user in _context.Users on assignTransfer.UserId equals user.Id
                      join workshop in _context.Workshop on assignment.WorkshopId equals workshop.Id
                      where assignTransfer.Id == request.AssignmentTransferRequestId
                      select new AssignmentTransferRequestDTO
                      {
                          TransferRequestId = assignTransfer.Id,
                          BatchCode = batch.Code,
                          ProductCode = product.Code,
                          UserName = user.FullName,
                          WorkshopName = workshop.Name,
                          CompletedQuantitySend = assignTransfer.CompletedQuantitySend,
                          CompletedQuantityReceive = assignTransfer.CompletedQuantityReceive,
                          Note = assignTransfer.Note ?? string.Empty,
                          NotLead = assignTransfer.NoteLead ?? string.Empty,
                          Status = assignTransfer.Status,
                          CreatedAt = assignTransfer.CreatedAt
                      };

            var assignmentTransferRequestDTO = await dto.FirstOrDefaultAsync(cancellationToken);

            if (assignmentTransferRequestDTO == null)
            {
                throw new NotFoundException("Yêu cầu đánh giá không tồn tại.");
            }

            return assignmentTransferRequestDTO;
        }
    }
}
