using System.Runtime.CompilerServices;
using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUserRepository _repository;
        private readonly IBatchRepository _batchRepository;
        private readonly IProductionRepository _productionRepository;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly ITaskTransferRequestRepository _taskTransferRequestRepository;
        private readonly IMaterialSupplyRepository _materialSupplyRepository;

        public DeleteUserCommandHandler(IUserRepository repository, IBatchRepository batchRepository, IProductionRepository productionRepository, IEvaluateRepository evaluateRepository,
            ITaskTransferRequestRepository taskTransferRequestRepository, IMaterialSupplyRepository materialSupplyRepository)
        {
            _repository = repository;
            _batchRepository = batchRepository;
            _productionRepository = productionRepository;
            _evaluateRepository = evaluateRepository;
            _taskTransferRequestRepository = taskTransferRequestRepository;
            _materialSupplyRepository = materialSupplyRepository;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id);
            if (user is null)
            {
                throw new NotFoundException($"Không tìm thấy User với Id: {request.Id}.");
            }

            if (user.Role == "GuardQC")
            {
                throw new ConflictException("Không thể xóa QC gác cổng.");
            }

            bool needSoftDelete = false;

            if (user.Role == "Lead")
            {
                var batches = await _batchRepository.GetBatchesByLeadIdAsync(user.Id);
                if (batches.Any())
                    needSoftDelete = true;
            }

            if (user.Role == "QC")
            {
                var evaluates = await _evaluateRepository.GetByQCIdAsync(user.Id);
                if (evaluates.Any())
                    needSoftDelete = true;
            }

            if (user.Role == "QCTransport")
            {
                var taskTransferRequest = await _taskTransferRequestRepository.GetByQcTransportIdAsync(user.Id);
                var materialSupply = await _materialSupplyRepository.GetByUserIdAsync(user.Id);
                if (taskTransferRequest.Any() || materialSupply.Any())
                {
                    needSoftDelete = true;
                }
            }

            var productions = await _productionRepository.GetByUserAsync(user.Id);
            if (productions.Any())
                needSoftDelete = true;

            if (needSoftDelete)
            {
                user.MarkAsDeleted();
            }
            else
            {
                _repository.Delete(user);
            }

            return Result.Success();
        }
    }
}
