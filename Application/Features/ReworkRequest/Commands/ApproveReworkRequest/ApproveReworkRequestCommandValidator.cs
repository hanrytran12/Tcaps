using FluentValidation;

namespace Application.Features.ReworkRequest.Commands.ApproveReworkRequest
{
    public class ApproveReworkRequestCommandValidator : AbstractValidator<ApproveReworkRequestCommand>
    {
        public ApproveReworkRequestCommandValidator()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            RuleFor(x => x.DeliveryDate)
                .NotEmpty()
                    .WithMessage("Ngày giao hàng sửa lỗi không được để trống.")
                .GreaterThanOrEqualTo(today)
                    .WithMessage("Ngày giao hàng sửa lỗi phải từ hôm nay trở đi.");

            RuleFor(x => x.EndDate)
                .NotEmpty()
                    .WithMessage("Ngày kết thúc sửa lỗi không được để trống.")
                .GreaterThan(x => x.DeliveryDate)
                    .WithMessage("Ngày kết thúc phải sau ngày giao hàng sửa lỗi.");

            RuleFor(x => x.NextStepDeliveryDate)
                .NotEmpty()
                    .WithMessage("Ngày giao bước tiếp theo không được để trống.")
                .GreaterThanOrEqualTo(x => x.EndDate)
                    .WithMessage("Ngày giao bước tiếp theo phải từ ngày kết thúc trở đi.");
        }
    }
}
