using Application.Common;
using Application.Interfaces;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MaterialRequest.Commands.DispatchRequest
{
    public class DispatchRequestCommandHandler : IRequestHandler<DispatchRequestCommand, Result>
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IAppDbContext _appDbContext;

        public DispatchRequestCommandHandler(IBatchRepository batchRepository, IMaterialRequestRepository materialRequestRepository, IAppDbContext appDbContext)
        {
            _batchRepository = batchRepository;
            _materialRequestRepository = materialRequestRepository;
            _appDbContext = appDbContext;
        }

        public async Task<Result> Handle(DispatchRequestCommand request, CancellationToken cancellationToken)
        {
            var batch = await _batchRepository.GetByAssignmentIdAsync(request.AssignmentId);

            if (batch is null)
            {
                return Result.Failure("Không tìm thấy lô hàng");
            }

            foreach (var item in request.Items)
            {
                var materialRequest = Domain.Entities.MaterialRequest.Create(item.MaterialId, request.UserId, batch.Id, request.AssignmentId, item.Quantity, "Yêu cầu xuất kho từ Lead");
                await _materialRequestRepository.AddAsync(materialRequest);

                var material = await _appDbContext.Materials.Where(m => m.Id == item.MaterialId).FirstOrDefaultAsync();
                materialRequest.AddDomainEvent(new MaterialRequestAddedEvent(item.Quantity, request.AssignmentId, material.Name, material.Unit));
            }

            return Result.Success();
        }
    }
}
