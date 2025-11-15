using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Queries.GetAllAsignmentByQCId
{
    public class GetAllAssignmentByQCIdQueryHandler : IRequestHandler<GetAllAssignmentByQCIdQuery, List<AssignForStaffDTO>>
    {
        private readonly IAppDbContext _context;

        public GetAllAssignmentByQCIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<AssignForStaffDTO>> Handle(GetAllAssignmentByQCIdQuery request, CancellationToken cancellationToken)
        {
            var qc = await _context.Users.FindAsync(request.QcId);
            if (qc == null)
                return new List<AssignForStaffDTO>();

            var assignments = await _context.Assignments
                .Where(a => a.WorkshopId == qc.WorkshopId)
                .ToListAsync();

            if (!assignments.Any())
                return new List<AssignForStaffDTO>();

            var batchIds = assignments.Select(a => a.BatchId).ToList();
            var batches = await _context.Batches
                .Where(b => batchIds.Contains(b.Id))
                .ToListAsync();

            // 4. Lấy toàn bộ sản phẩm liên quan (tránh query trong vòng lặp)
            var productIds = batches.Select(b => b.ProductId).Distinct().ToList();
            var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

            // 5. Map nhanh bằng LINQ (KHÔNG ASYNC)
            var dtos = (from a in assignments
                        join b in batches on a.BatchId equals b.Id
                        join p in products on b.ProductId equals p.Id
                        select new AssignForStaffDTO
                        {
                            AssignId = a.Id,
                            BatchId = a.BatchId,
                            BatchesCode = b.Code,
                            ProductCode = p.Code,
                            WorkshopId = a.WorkshopId,
                            StepOrder = a.StepOrder,
                            Quantity = a.Quantity,
                            StartDate = a.StartDate,
                            EndDate = a.EndDate,
                            ExpectedDeliveryDate = a.ExpectedDeliveryDate,
                            UnitPrice = a.UnitPrice,
                            Status = a.Status
                        })
                        .OrderByDescending(x => x.StartDate)
                        .ToList();

            return dtos;
        }
    }
}
