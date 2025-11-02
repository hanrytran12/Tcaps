using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Assignments.Queries
{
    public class GetAssignmentsByStaffIdQueryHandler : IRequestHandler<GetAssignmentsByStaffIdQuery, List<AssignForStaffDTO>>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public GetAssignmentsByStaffIdQueryHandler(IAssignmentRepository assignmentRepository, IBatchRepository batchRepository,
            IMapper mapper, IUserRepository userRepository)
        {
            _assignmentRepository = assignmentRepository;
            _batchRepository = batchRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<List<AssignForStaffDTO>> Handle(GetAssignmentsByStaffIdQuery request, CancellationToken cancellationToken)
        {
            var staff = await _userRepository.GetByIdAsync(request.StaffId);
            if (staff == null)
                throw new Exception("Staff không tìm thấy.");

            if (staff.WorkshopId == null)
                throw new Exception("Staff không có xưởng liên quan.");

            var assignments = await _assignmentRepository.GetAssignmentsAsync(staff.WorkshopId);
            if (assignments == null || !assignments.Any())
                return new List<AssignForStaffDTO>();

            var batchIds = assignments.Select(x => x.BatchId).ToList();
            var batches = await _batchRepository.GetBatchesByIdsAsync(batchIds);

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
