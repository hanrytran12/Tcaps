using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AssingmentTransferRequest.Queries.GetAllForQcTransport
{
    public class GetAllForQcTransportQueryHandler : IRequestHandler<GetAllForQcTransportQuery, Result<List<AssignmentTransferRequestDTO>>>
    {
        private readonly IAppDbContext _context;

        public GetAllForQcTransportQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<AssignmentTransferRequestDTO>>> Handle(GetAllForQcTransportQuery request, CancellationToken cancellationToken)
        {
            var qc = await _context.Users.
                FindAsync(request.QcTransportId);

            if (qc == null)
            {
                return Result<List<AssignmentTransferRequestDTO>>.Failure("Không tìm thấy QC vận chuyển");
            }

            var assignmentTransfer = from assignTransfer in _context.AssignmentTransferRequests

                                     join taskTransfer in _context.TaskTransferRequests 
                                     on assignTransfer.Id equals taskTransfer.AssignmentTransferId

                                     join assignment in _context.Assignments 
                                     on assignTransfer.AssignmentId equals assignment.Id

                                     join workshop in _context.Workshop 
                                     on assignment.WorkshopId equals workshop.Id

                                     join batch in _context.Batches 
                                     on assignment.BatchId equals batch.Id

                                     join product in _context.Products
                                     on batch.ProductId equals product.Id

                                     join user in _context.Users 
                                     on assignTransfer.UserId equals user.Id

                                     where taskTransfer.QcTransportId == request.QcTransportId
                                     select new AssignmentTransferRequestDTO
                                     {
                                         TransferRequestId = assignTransfer.Id,
                                         BatchCode = batch.Code,
                                         ProductCode = product.Code,
                                         WorkshopName = workshop.Name,
                                         CompletedQuantity = assignTransfer.CompletedQuantity,
                                         Note = assignTransfer.Note,
                                         Status = assignTransfer.Status,
                                         CreatedAt = assignTransfer.CreatedAt
                                     };

            var result = await assignmentTransfer.ToListAsync(cancellationToken);
            return Result<List<AssignmentTransferRequestDTO>>.Success(result);
        }
    }
}
