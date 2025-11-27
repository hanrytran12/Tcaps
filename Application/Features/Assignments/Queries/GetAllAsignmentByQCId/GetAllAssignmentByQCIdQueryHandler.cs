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
            var query = from user in _context.Users.AsNoTracking() // Không tracking để đọc nhanh hơn
                        where user.Id == request.QcId

                        // 1. Join User -> Assignment (qua WorkshopId)
                        join assign in _context.Assignments.AsNoTracking()
                            on user.WorkshopId equals assign.WorkshopId

                        // 2. Join Assignment -> Batch
                        join batch in _context.Batches.AsNoTracking()
                            on assign.BatchId equals batch.Id

                        // 3. Join Batch -> Product
                        join product in _context.Products.AsNoTracking()
                            on batch.ProductId equals product.Id

                        // 4. Sắp xếp ngay tại SQL Server (Tối ưu Index nếu có)
                        orderby assign.StartDate descending

                        // 5. Projection: Chỉ SELECT các cột cần thiết ra DTO
                        select new AssignForStaffDTO
                        {
                            AssignId = assign.Id,
                            BatchId = assign.BatchId,
                            BatchesCode = batch.Code,
                            ProductCode = product.Code,
                            WorkshopId = assign.WorkshopId,
                            StepOrder = assign.StepOrder,
                            Quantity = assign.Quantity,
                            StartDate = assign.StartDate,
                            EndDate = assign.EndDate,
                            DateCompleted = assign.DateCompleted,
                            ExpectedDeliveryDate = assign.ExpectedDeliveryDate,
                            UnitPrice = assign.UnitPrice,
                            Status = assign.Status
                        };

            var result = await query.ToListAsync(cancellationToken);

            return result;
        }
    }
}
