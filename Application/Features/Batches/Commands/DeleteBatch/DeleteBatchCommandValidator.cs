using FluentValidation;

namespace Application.Features.Batches.Commands.DeleteBatch
{
    public class DeleteBatchCommandValidator : AbstractValidator<DeleteBatchCommand>
    {
        public DeleteBatchCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Mã lô hàng không được để trống.");
        }
    }
}
