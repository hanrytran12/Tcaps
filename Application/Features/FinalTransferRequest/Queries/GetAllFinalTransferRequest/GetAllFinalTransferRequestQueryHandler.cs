using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.FinalTransferRequest.Queries.GetAllFinalTransferRequest
{
    public class GetAllFinalTransferRequestQueryHandler : IRequestHandler<GetAllFinalTransferRequestQuery, List<FinalTransferRequestDTO>>
    {
        private readonly IAppDbContext _context;

        public GetAllFinalTransferRequestQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<FinalTransferRequestDTO>> Handle(GetAllFinalTransferRequestQuery request, CancellationToken cancellationToken)
        {
            var dtos = await (from f in _context.FinalTransferRequests
                       join a in _context.AssignmentTransferRequests
                            on f.AssignTransferRequestId equals a.Id
                       join assign in _context.Assignments
                            on a.AssignmentId equals assign.Id
                       join b in _context.Batches
                            on assign.BatchId equals b.Id
                       select new FinalTransferRequestDTO
                       {
                           Id = f.Id,
                           AssignTransferRequestId = f.AssignTransferRequestId,
                           BatchCode = b.Code,
                           QuantityFinalSend = f.QuantityFinalSend,
                           QuantityFinalReceive = f.QuantityFinalReceive,
                           Status = f.Status,
                           Note = f.Note,
                           ApprovedNote = f.ApprovedNote,
                           CreatedAt = f.CreatedAt,
                           ApprovedAt = f.ApprovedAt
                       })
                       .GroupBy(x => x.Id)
                       .Select(g => g.First())
                       .ToListAsync();

            if (!dtos.Any())
            {
                throw new NotFoundException("Không có yêu cầu nào.");
            }

            return dtos;
        }
    }
}
