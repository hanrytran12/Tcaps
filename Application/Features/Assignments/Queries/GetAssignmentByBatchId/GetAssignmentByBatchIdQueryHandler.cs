using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Queries.GetAssignmentByBatchId
{
    public class GetAssignmentByBatchIdQueryHandler : IRequestHandler<GetAssignmentByBatchIdQuery, AssignForStaffDTO>
    {
        private readonly IAppDbContext _context;

        public GetAssignmentByBatchIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<AssignForStaffDTO> Handle(GetAssignmentByBatchIdQuery request, CancellationToken cancellationToken)
        {
            var query = from user in _context.Users.AsNoTracking()
                        where user.Id == request.StaffId
                        join assign in _context.Assignments.AsNoTracking()
                            on user.WorkshopId equals assign.WorkshopId
                        where assign.BatchId == request.BatchId
                        join batch in _context.Batches.AsNoTracking()
                            on assign.BatchId equals batch.Id
                        join product in _context.Products.AsNoTracking()
                            on batch.ProductId equals product.Id
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
                throw new NotFoundException("Assignment not found or Staff does not belong to the Workshop.");
            }
            return result;
        }
    }
}
