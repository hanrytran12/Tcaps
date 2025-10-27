using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Assignments.Commands.AddAssignmentCommand
{
    public class AddAssignmentCommandHandler : IRequestHandler<AddAssignmentCommand, Result<Guid>>
    {
        private readonly IBatchRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkshopRepository _workshopRepository;

        public AddAssignmentCommandHandler(IBatchRepository repository, IUnitOfWork unitOfWork, IWorkshopRepository workshopRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _workshopRepository = workshopRepository;
        }

        public async Task<Result<Guid>> Handle(AddAssignmentCommand request, CancellationToken cancellationToken)
        {
            var batch = await _repository.GetByIdAsync(request.BatchId);
            if (batch is null)
            {
                return Result<Guid>.Failure("Batch is not exist.");
            }

            var workshop = await _workshopRepository.GetByIdAsync(request.WorkshopId);
            if (workshop is null)
            {
                return Result<Guid>.Failure("Workshop is not exist.");
            }

            var assignment = Assignment.Create(request.BatchId, request.WorkshopId, request.Quantity, request.StartDate, request.EndDate, request.ExpectedDeliveryDate);
            batch.AddAssignment(assignment);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(assignment.Id);
        }
    }
}
