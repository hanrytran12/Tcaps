using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Queries.GetTaskProgressByQCId
{
    public class GetTaskProgressByQCIdQueryHandler : IRequestHandler<GetTaskProgressByQCIdQuery, List<TaskProgressDTO>>
    {
        private readonly IAppDbContext _context;

        public GetTaskProgressByQCIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<TaskProgressDTO>> Handle(GetTaskProgressByQCIdQuery request, CancellationToken cancellationToken)
        {
            var rawData = await (from a in _context.Assignments
                                 join u in _context.Users on a.WorkshopId equals u.WorkshopId
                                 join b in _context.Batches on a.BatchId equals b.Id
                                 join p in _context.Products on b.ProductId equals p.Id
                                 join pro in _context.Productions.AsNoTracking() 
                                    on a.Id equals pro.AssignId into proGroup
                                 from pro in proGroup.DefaultIfEmpty()
                                 join e in _context.Evaluates.AsNoTracking() 
                                    on pro.Id equals e.ProductionId into evalGroup
                                 from e in evalGroup.DefaultIfEmpty()
                                 join assignTransfer in _context.AssignmentTransferRequests
                                    on a.Id equals assignTransfer.AssignmentId into atrGroup
                                 from atr in atrGroup.DefaultIfEmpty()
                                 where u.Id == request.QcId
                                 select new
                                 {
                                     AssignmentId = a.Id,
                                     a.BatchId,
                                     BatchCode = b.Code,
                                     ProductCode = p.Code,
                                     ProductName = p.Name,
                                     a.StartDate,
                                     a.EndDate,
                                     a.Status,
                                     a.UnitPrice,
                                     a.CreatedAt,
                                     QuantityRequest = a.Quantity,
                                     ProductionId = pro != null ? pro.Id : (Guid?)null,
                                     EvaluateId = e != null ? e.Id : (Guid?)null,
                                     EvaluateStatus = e != null ? e.Status : null,
                                     QuantitySuccess = e != null ? e.QuantitySuccess : 0,
                                     QuantityError = e != null ? e.QuantityError : 0,

                                     QuantitySend = atr != null ? atr.CompletedQuantitySend : 0,
                                     QuantityReceive = atr != null ? atr.CompletedQuantityReceive : 0
                                 })
        .ToListAsync(cancellationToken);

            var assignmentIds = rawData.Select(x => x.AssignmentId).Distinct().ToList();
            var evaluateIds = rawData.Where(x => x.EvaluateId.HasValue).Select(x => x.EvaluateId.Value).Distinct().ToList();

            // Lấy ComponentDefects riêng
            var componentDefects = await _context.ComponentDefects
                .AsNoTracking()
                .Where(cd => evaluateIds.Contains(cd.EvaluateId))
                .GroupBy(cd => cd.EvaluateId)
                .Select(g => new
                {
                    EvaluateId = g.Key,
                    ConfirmedQuantity = g.Where(x => x.Status == "Confirmed").Sum(x => x.Quantity),
                    UnfixableQuantity = g.Where(x => x.Status == "Unfixable").Sum(x => x.Quantity),
                    HasUnfixable = g.Any(x => x.Status == "Unfixable")
                })
                .ToListAsync(cancellationToken);

            // Lấy ReworkRequests riêng
            var reworkRequests = await _context.ReworkRequests
                .AsNoTracking()
                .Where(r => assignmentIds.Contains(r.AssignmentId) && r.Status != "PendingLead")
                .GroupBy(r => r.AssignmentId)
                .Select(g => new
                {
                    AssignmentId = g.Key,
                    TotalReworkQuantity = g.Sum(x => x.DefectiveQuantity)
                })
                .ToListAsync(cancellationToken);

            var dto = rawData
                .GroupBy(x => new
                {
                    x.AssignmentId,
                    x.BatchId,
                    x.BatchCode,
                    x.ProductCode,
                    x.ProductName,
                    x.StartDate,
                    x.EndDate,
                    x.Status,
                    x.UnitPrice,
                    x.CreatedAt,
                    x.QuantityRequest
                })
                .Select(g =>
                {
                    var evaluates = g.Where(x => x.EvaluateId.HasValue).ToList();

                    int quantityCompleted = 0;
                    int quantityError = 0;

                    foreach (var eval in evaluates)
                    {
                        var defect = componentDefects.FirstOrDefault(cd => cd.EvaluateId == eval.EvaluateId);

                        if (eval.EvaluateStatus == "Passed" || eval.EvaluateStatus == "Rejected")
                        {
                            // Passed hoặc Rejected: lấy QuantitySuccess
                            quantityCompleted += eval.QuantitySuccess;

                            if (eval.EvaluateStatus == "Rejected")
                            {
                                quantityError += eval.QuantityError;
                            }
                        }
                        else if (eval.EvaluateStatus == "Failed")
                        {
                            if (defect != null)
                            {
                                if (defect.HasUnfixable)
                                {
                                    // CÓ Unfixable: Completed = Success + Confirmed, Error = Unfixable
                                    quantityCompleted += eval.QuantitySuccess + defect.ConfirmedQuantity;
                                    quantityError += defect.UnfixableQuantity;
                                }
                                else
                                {
                                    // KHÔNG có Unfixable: Completed = Success + Confirmed
                                    quantityCompleted += defect.ConfirmedQuantity + eval.QuantitySuccess;
                                }
                            }
                            else
                            {
                                // Chưa có defect: Completed = Success
                                quantityCompleted += eval.QuantitySuccess;
                            }
                        }
                    }

                    var rework = reworkRequests.FirstOrDefault(r => r.AssignmentId == g.Key.AssignmentId);

                    var transferDiff = g.Sum(x =>
                    {
                        var diff = x.QuantitySend - x.QuantityReceive;
                        return diff > 0 ? diff : 0;
                    });
                    quantityCompleted = (int)Math.Max(quantityCompleted - transferDiff, 0);
                    return new TaskProgressDTO
                    {
                        AssignmentId = g.Key.AssignmentId,
                        BatchId = g.Key.BatchId,
                        BatchCode = g.Key.BatchCode,
                        ProductCode = g.Key.ProductCode,
                        ProductName = g.Key.ProductName,
                        StartDate = g.Key.StartDate,
                        EndDate = g.Key.EndDate,
                        Status = g.Key.Status,
                        UnitPrice = g.Key.UnitPrice,
                        CreatedAt = g.Key.CreatedAt,
                        TaskMetricsDTO = new TaskMetricsDTO
                        {
                            QuantityRequest = g.Key.QuantityRequest,
                            QuantityCompleted = quantityCompleted,
                            QuantityError = quantityError,
                            QuantityRework = rework?.TotalReworkQuantity ?? 0
                        }
                    };
                })
                .ToList();
            return dto!;
        }
    }
}
