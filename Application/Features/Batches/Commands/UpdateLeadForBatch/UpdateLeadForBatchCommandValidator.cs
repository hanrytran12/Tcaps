using FluentValidation;

namespace Application.Features.Batches.Commands.UpdateLeadForBatch
{
    public class UpdateLeadForBatchCommandValidator : AbstractValidator<UpdateLeadForBatchCommand>
    {
        public UpdateLeadForBatchCommandValidator()
        {
            RuleFor(x => x.BatchId)
                .NotEmpty().WithMessage("Mã lô hàng không được để trống.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Mã Lead không được để trống.");
        }
    }
}
