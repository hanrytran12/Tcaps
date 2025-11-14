using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Features.Assignments.Queries.NewFolder;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Features.Assignments.Queries.GetAssignmentForHistoryByBatchId
{
    public class GetAssignmentForHistoryByBatchIdQueryHandler : IRequestHandler<GetAssignmentForHistoryByBatchIdQuery, Result<List<AssignmentHistoryDTO>>>
    {
        private readonly IAppDbContext _context;

        public GetAssignmentForHistoryByBatchIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<AssignmentHistoryDTO>>> Handle(GetAssignmentForHistoryByBatchIdQuery request, CancellationToken cancellationToken)
        {
            // 1) Lấy thông tin QC trước (để lấy WorkshopId)
            var qc = await _context.Users
                .Where(u => u.Id == request.QcId)
                .Select(u => new { u.Id, u.WorkshopId })
                .FirstOrDefaultAsync(cancellationToken);

            if (qc == null)
                return Result<List<AssignmentHistoryDTO>>.Failure("QC not found");

            // 2) Lấy các staff thuộc cùng workshop với QC
            var staffInWorkshop = await _context.Users
                .Where(u => u.WorkshopId == qc.WorkshopId && u.Role == "Staff")
                .Select(u => new { u.Id, u.FullName })
                .ToListAsync(cancellationToken);

            var staffIds = staffInWorkshop.Select(x => x.Id).ToList();

            // 3) Join Assignments + Productions + Staff theo điều kiện đúng
            var productions = await (
                from a in _context.Assignments
                join p in _context.Productions on a.Id equals p.AssignId
                join u in _context.Users on p.UserId equals u.Id
                where a.BatchId == request.BatchId
                      && a.WorkshopId == qc.WorkshopId
                      && staffIds.Contains(p.UserId) // Staff phải cùng workshop với QC
                select new
                {
                    Assignment = a,
                    ProductionId = p.Id,
                    StaffName = u.FullName,
                    QuantityWork = p.Quantity
                }
            ).ToListAsync(cancellationToken);

            // 4) Lấy lỗi UNFIXABLE
            var defects = await (
                from p in _context.Productions
                join e in _context.Evaluates on p.Id equals e.ProductionId
                join c in _context.ComponentDefects on e.Id equals c.EvaluateId
                where c.Status == "Unfixable"
                select new
                {
                    ProductionId = p.Id,
                    ErrorQuantity = c.Quantity
                }
            ).ToListAsync(cancellationToken);

            var errorDictionary = defects
                .GroupBy(x => x.ProductionId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.ErrorQuantity)
                );

            // 5) Merge dữ liệu thành DTO
            var result = productions
                .GroupBy(x => x.Assignment)
                .Select(g => new AssignmentHistoryDTO
                {
                    WorkshopId = g.Key.WorkshopId,
                    StepOrder = g.Key.StepOrder,
                    QuantityOrder = g.Key.Quantity,
                    UnitPrice = g.Key.UnitPrice,
                    StartDate = g.Key.StartDate,
                    EndDate = g.Key.EndDate,
                    ExpectedDeliveryDate = g.Key.ExpectedDeliveryDate,
                    Status = g.Key.Status,

                    Items = g
                        .GroupBy(i => i.StaffName)
                        .Select(s => new StaffWorksingDTO
                        {
                            StaffName = s.Key,
                            QuantityWork = s.Sum(x => x.QuantityWork),
                            QuantityError = s.Sum(x =>
                                errorDictionary.ContainsKey(x.ProductionId)
                                    ? errorDictionary[x.ProductionId]
                                    : 0
                            )
                        }).ToList()
                })
                .ToList();


            return Result<List<AssignmentHistoryDTO>>.Success(result);
        }
    }
}
