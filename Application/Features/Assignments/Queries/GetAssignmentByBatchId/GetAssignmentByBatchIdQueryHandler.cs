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

        public GetAssignmentByBatchIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<Result<AssignForStaffDTO>> Handle(GetAssignmentByBatchIdQuery request, CancellationToken cancellationToken)
        {
            //var staff = await _context.Users.FindAsync(request.StaffId);
            //if (staff == null)
            //    return Result<AssignForStaffDTO>.Failure("Staff not found.");
            //var batch = await _context.Batches.FindAsync(request.BatchId);

            //var product = await _context.Products.FindAsync(batch.ProductId);
            //var assignment = await _context.Assignments
            //    .FirstOrDefaultAsync(a => a.BatchId == batch.Id
            //             && a.WorkshopId == staff.WorkshopId);
            //if (assignment == null)
            //    return Result<AssignForStaffDTO>.Failure("Assignment not found.");

            //var dto = new AssignForStaffDTO
            //{
            //    AssignId = assignment.Id,
            //    BatchId = assignment.BatchId,
            //    BatchesCode = batch.Code,
            //    ProductCode = product.Code,
            //    WorkshopId = assignment.WorkshopId,
            //    StepOrder = assignment.StepOrder,
            //    Quantity = assignment.Quantity,
            //    UnitPrice = assignment.UnitPrice,
            //    StartDate = assignment.StartDate,
            //    EndDate = assignment.EndDate,
            //    ExpectedDeliveryDate = assignment.ExpectedDeliveryDate,
            //    Status = assignment.Status
            //};

            var query = from user in _context.Users.AsNoTracking()
                        where user.Id == request.StaffId

                        // Join Assignment theo WorkshopId của User
                        join assign in _context.Assignments.AsNoTracking()
                            on user.WorkshopId equals assign.WorkshopId
                        where assign.BatchId == request.BatchId

                        // Join Batch để lấy Code
                        join batch in _context.Batches.AsNoTracking()
                            on assign.BatchId equals batch.Id

                        // Join Product để lấy Code
                        join product in _context.Products.AsNoTracking()
                            on batch.ProductId equals product.Id

                        // Projection: Select trực tiếp ra DTO (SQL sẽ chỉ SELECT các cột cần thiết)
                        select new AssignForStaffDTO
                        {
                            AssignId = assign.Id,
                            BatchId = assign.BatchId,
                            BatchesCode = batch.Code,
                            ProductCode = product.Code,
                            WorkshopId = assign.WorkshopId,
                            StepOrder = assign.StepOrder,
                            Quantity = assign.Quantity,
                            UnitPrice = assign.UnitPrice,
                            StartDate = assign.StartDate,
                            EndDate = assign.EndDate,
                            DateCompleted = assign.DateCompleted,
                            ExpectedDeliveryDate = assign.ExpectedDeliveryDate,
                            Status = assign.Status
                        };

            var result = await query.FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                return Result<AssignForStaffDTO>.Failure("Assignment not found or Staff does not belong to the Workshop.");
            }

            return Result<AssignForStaffDTO>.Success(result);
        }
    }
}
