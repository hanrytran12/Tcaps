using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Assignments.Queries.GetAssignmentsByStaffId
{
    public class GetAssignmentsByStaffIdQueryHandler : IRequestHandler<GetAssignmentsByStaffIdQuery, List<AssignForStaffDTO>>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IAppDbContext _context;

        public GetAssignmentsByStaffIdQueryHandler(IAssignmentRepository assignmentRepository, IBatchRepository batchRepository,
            IMapper mapper, IUserRepository userRepository, IProductRepository productRepository, IAppDbContext context)
        {
            _assignmentRepository = assignmentRepository;
            _batchRepository = batchRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _context = context;
        }
        public async Task<List<AssignForStaffDTO>> Handle(GetAssignmentsByStaffIdQuery request, CancellationToken cancellationToken)
        {
            var staff = await _context.Users.FindAsync(request.StaffId);
            if (staff == null)
                throw new Exception("Staff không tìm thấy.");

            if (staff.WorkshopId == null)
                throw new Exception("Staff không có xưởng liên quan.");

            var assignments = await _context.Assignments
                .Where(a => a.WorkshopId == staff.WorkshopId)
                .ToListAsync(cancellationToken);
            if (assignments == null || !assignments.Any())
                return new List<AssignForStaffDTO>();

            var batchIds = assignments.Select(x => x.BatchId).ToList();

            var batches = await _context.Batches
                .Where(b => batchIds.Contains(b.Id))
                .ToListAsync(cancellationToken);

            // 4. Lấy toàn bộ sản phẩm liên quan (tránh query trong vòng lặp)
            var productIds = batches.Select(b => b.ProductId).Distinct().ToList();

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync(cancellationToken);

            // 5. Map nhanh bằng LINQ (KHÔNG ASYNC)
            var dtos = (from a in assignments
                        join b in batches on a.BatchId equals b.Id
                        join p in products on b.ProductId equals p.Id
                        select new AssignForStaffDTO
                        {
                            AssignId = a.Id,
                            BatchId = a.BatchId,
                            BatchesCode = b.Code,
                            ProductCode = p.Code,
                            WorkshopId = a.WorkshopId,
                            StepOrder = a.StepOrder,
                            Quantity = a.Quantity,
                            StartDate = a.StartDate,
                            EndDate = a.EndDate,
                            ExpectedDeliveryDate = a.ExpectedDeliveryDate,
                            UnitPrice = a.UnitPrice,
                            Status = a.Status
                        })
                        .OrderByDescending(x => x.StartDate)
                        .ToList();

            return dtos;
        }
    }
}
