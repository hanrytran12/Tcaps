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

            var batchIds = assignments.Select(a => a.BatchId).ToList();
            var batches = await _context.Batches
                .Where(b => batchIds.Contains(b.Id))
                .ToListAsync();

            var dtos = assignments.Select(a =>
            {
                var batch = batches.FirstOrDefault(b => b.Id == a.BatchId);
                return new AssignForStaffDTO
                {
                    AssignId = a.Id,
                    BatchId = a.BatchId,
                    BatchesCode = batch?.Code, // tránh null
                    WorkshopId = a.WorkshopId,
                    StepOrder = a.StepOrder,
                    Quantity = a.Quantity,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    ExpectedDeliveryDate = a.ExpectedDeliveryDate,
                    UnitPrice = a.UnitPrice
                };
            })
            .OrderByDescending(a => a.StartDate)
            .ToList();

            return dtos;
        }
    }
}
