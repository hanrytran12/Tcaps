using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Queries.GetAssignmentByBatchId
{
    public class GetAssignmentByBatchIdQueryHandler : IRequestHandler<GetAssignmentByBatchIdQuery, Result<AssignForStaffDTO>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public GetAssignmentByBatchIdQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<AssignForStaffDTO>> Handle(GetAssignmentByBatchIdQuery request, CancellationToken cancellationToken)
        {
            var staff = await _context.Users.FindAsync(request.StaffId);
            if (staff == null)
                return Result<AssignForStaffDTO>.Failure("Staff not found.");
            var batch = await _context.Batches.FindAsync(request.BatchId);

            var product = await _context.Products.FindAsync(batch.ProductId);
            var assignment = await _context.Assignments
                .FirstOrDefaultAsync(a => a.BatchId == batch.Id
                         && a.WorkshopId == staff.WorkshopId);
            if (assignment == null)
                return Result<AssignForStaffDTO>.Failure("Assignment not found.");

            var dto = new AssignForStaffDTO
            {
                AssignId = assignment.Id,
                BatchId = assignment.BatchId,
                BatchesCode = batch.Code,
                ProductCode = product.Code,
                WorkshopId = assignment.WorkshopId,
                StepOrder = assignment.StepOrder,
                Quantity = assignment.Quantity,
                UnitPrice = assignment.UnitPrice,
                StartDate = assignment.StartDate,
                EndDate = assignment.EndDate,
                ExpectedDeliveryDate = assignment.ExpectedDeliveryDate,
                Status = assignment.Status
            };
            return Result<AssignForStaffDTO>.Success(dto);
        }
    }
}
