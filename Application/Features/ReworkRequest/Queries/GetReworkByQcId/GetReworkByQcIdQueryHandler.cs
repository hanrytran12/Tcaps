using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ReworkRequest.Queries.GetReworkByQcId
{
    public class GetReworkByQcIdQueryHandler : IRequestHandler<GetReworkByQcIdQuery, List<ReworkRequestDTO>>
    {
        private readonly IAppDbContext _context;

        public GetReworkByQcIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ReworkRequestDTO>> Handle(GetReworkByQcIdQuery request, CancellationToken cancellationToken)
        {
            var dtos = await (from r in _context.ReworkRequests
                             join assign in _context.Assignments on r.AssignmentId equals assign.Id
                             join w in _context.Workshop on assign.WorkshopId equals w.Id
                             join u in _context.Users on w.Id equals u.WorkshopId
                             join b in _context.Batches on assign.BatchId equals b.Id
                             where u.Id == request.QcId
                             select new ReworkRequestDTO
                             {
                                 Id = r.Id,
                                 BatchCode = b.Code,
                                 QcName = u.FullName,
                                 WorkshopId = w.Id,
                                 WorkshopName = w.Name,
                                 AssignmentId = assign.Id,
                                 DefectiveQuantity = r.DefectiveQuantity,
                                 NoteQc = r.NoteQc,
                                 Status = r.Status,
                                 RequiresMaterialDelivery = assign.RequiresMaterialDelivery,
                                 CreatedAt = r.CreatedAt,
                                 DeliveryDate = r.DeliveryDate,
                                 EndDate = r.EndDate,
                                 NextStepDeliveryDate = r.NextStepDeliveryDate
                             })
                             .ToListAsync();

            return dtos;
        }
    }
}
