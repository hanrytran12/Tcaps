using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Queries.GetTaskProgressByQCId
{
    public class GetTaskProgressByQCIdQueryHandler : IRequestHandler<GetTaskProgressByQCIdQuery, TaskProgressDTO>
    {
        private readonly IAppDbContext _context;

        public GetTaskProgressByQCIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<TaskProgressDTO> Handle(GetTaskProgressByQCIdQuery request, CancellationToken cancellationToken)
        {
            var rawData = await (from a in _context.Assignments
                             join u in _context.Users on a.WorkshopId equals u.WorkshopId
                             join b in _context.Batches on a.BatchId equals b.Id
                             join p in _context.Products on b.ProductId equals p.Id
                             join pro in _context.Productions on a.Id equals pro.AssignId
                             join e in _context.Evaluates on pro.Id equals e.ProductionId
                             join c in _context.ComponentDefects on e.Id equals c.EvaluateId into defectGroup
                             from d in defectGroup.DefaultIfEmpty()
                             join r in _context.ReworkRequests on a.Id equals r.AssignmentId into reworkGroup
                             from r in reworkGroup.DefaultIfEmpty()
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
                                 QuantityRequest = a.Quantity,

                                 EvaluateStatus = e.Status,
                                 e.QuantitySuccess,
                                 e.QuantityError,

                                 DefectStatus = d != null ? d.Status : null,
                                 DefectQuantity = d != null ? d.Quantity : 0,

                                 ReworkStatus = r != null ? r.Status : null,
                                 ReworkQuantity = r != null ? r.DefectiveQuantity : 0
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
                    x.QuantityRequest
                })
                .Select(g => new TaskProgressDTO
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

                    TaskMetricsDTO = new TaskMetricsDTO
                    {
                        QuantityRequest = g.Key.QuantityRequest,

                        // ✔ QuantityCompleted = QuantitySuccess + ComponentDefect(Rework)
                        QuantityCompleted =
                            g.GroupBy(x => new
                            {
                                x.EvaluateStatus,
                                x.QuantitySuccess
                            })
                            .Sum(eg => eg.Key.QuantitySuccess)
                                + g.Where(x => x.DefectStatus == "Rework")
                            .Sum(x => x.DefectQuantity),


                        // ✔ QuantityError
                        QuantityError =
                            g.Where(x => x.EvaluateStatus == "Rejected")
                             .Sum(x => x.QuantityError)
                          + g.Where(x => x.EvaluateStatus == "Failed"
                                      && x.DefectStatus == "Unfixabled")
                             .Sum(x => x.DefectQuantity),

                        // ✔ QuantityRework
                        QuantityRework =
                            g.Where(x => x.ReworkStatus == "Approved")
                             .Sum(x => x.ReworkQuantity)
                    }
                })
                .FirstOrDefault();

            return dto!;
        }
    }
}
