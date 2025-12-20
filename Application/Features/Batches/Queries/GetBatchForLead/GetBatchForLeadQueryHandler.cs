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

namespace Application.Features.Batches.Queries.GetBatchForLead
{
    public class GetBatchForLeadQueryHandler : IRequestHandler<GetBatchForLeadQuery, List<BatchDTO>>
    {
        private readonly IAppDbContext _context;

        public GetBatchForLeadQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<BatchDTO>> Handle(GetBatchForLeadQuery request, CancellationToken cancellationToken)
        {
            var dtos = await (from b in _context.Batches
                              join u in _context.Users on b.UserId equals u.Id
                              join p in _context.Products on b.ProductId equals p.Id
                              where b.UserId == request.UserId
                              select new BatchDTO
                              {
                                  BatchId = b.Id,
                                  UserId = b.UserId ?? Guid.Empty,
                                  LeadName = u.FullName,
                                  ProductCode = p.Code,
                                  ProductName = p.Name,
                                  Code = b.Code,
                                  Quantity = b.Quantity,
                                  StartDate = b.StartDate,
                                  EndDate = b.EndDate,
                                  Status = b.Status,
                                  CreatedAt = b.CreatedAt
                              }).ToListAsync();

            if (!dtos.Any())
            {
                return new List<BatchDTO>();
            }

            return dtos;
        }
    }
}
