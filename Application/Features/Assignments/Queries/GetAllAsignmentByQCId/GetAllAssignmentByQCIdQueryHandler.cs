using Application.DTOs.Response;
using Application.Interfaces;
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
            var query = from user in _context.Users.AsNoTracking()
                        where user.Id == request.QcId
                        join assign in _context.Assignments.AsNoTracking()
                            on user.WorkshopId equals assign.WorkshopId
                        join batch in _context.Batches.AsNoTracking()
                            on assign.BatchId equals batch.Id
                        join product in _context.Products.AsNoTracking()
                            on batch.ProductId equals product.Id
                        orderby assign.StartDate descending
                        select new AssignForStaffDTO
                        {
                            AssignId = assign.Id,
                            BatchId = assign.BatchId,
                            BatchesCode = batch.Code,
                            ProductCode = product.Code,
                            ProductName = product.Name,
                            WorkshopId = assign.WorkshopId,
                            StepOrder = assign.StepOrder,
                            Quantity = assign.Quantity,
                            StartDate = assign.StartDate,
                            EndDate = assign.EndDate,
                            DateCompleted = assign.DateCompleted,
                            ExpectedDeliveryDate = assign.ExpectedDeliveryDate,
                            UnitPrice = assign.UnitPrice,
                            Status = assign.Status,
                            IsFinalWorkshop = _context.Assignments
                                                .Where(a => a.BatchId == assign.BatchId)
                                                .Max(a => a.StepOrder) == assign.StepOrder
                        };
            var result = await query.ToListAsync(cancellationToken);
            return result;
        }
    }
}
