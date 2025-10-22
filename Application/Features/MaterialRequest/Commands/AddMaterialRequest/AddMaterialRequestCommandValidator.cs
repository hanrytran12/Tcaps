using Domain.Interfaces;
using FluentValidation;

namespace Application.Features.MaterialRequest.Commands.AddMaterialRequest
{
    public class AddMaterialRequestCommandValidator : AbstractValidator<AddMaterialRequestCommand>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBatchRepository _batchRepository;

        public AddMaterialRequestCommandValidator(IMaterialRepository materialRepository, IUserRepository userRepository, IBatchRepository batchRepository)
        {
            _materialRepository = materialRepository;
            _userRepository = userRepository;
            _batchRepository = batchRepository;

            RuleFor(x => x.MaterialId)
                .NotEmpty().WithMessage("MaterialId is required.")
                .MustAsync(async (id, token) => await _materialRepository.GetByIdAsync(id) is not null)
                .WithMessage("Material does not exist.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .MustAsync(async (id, token) => await _userRepository.GetByIdAsync(id) is not null)
                .WithMessage("User does not exist.");

            RuleFor(x => x.BatchId)
                .NotEmpty().WithMessage("BatchId is required.")
                .MustAsync(async (id, token) => await _batchRepository.GetByIdAsync(id) is not null)
                .WithMessage("Batch does not exist.");

            RuleFor(x => x.QuantityRequest)
                .GreaterThan(0).WithMessage("QuantityRequest must be greater than zero.");
        }
    }
}
