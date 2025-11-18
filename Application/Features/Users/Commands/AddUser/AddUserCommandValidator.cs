using Domain.Interfaces;
using FluentValidation;

namespace Application.Features.Users.Commands.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IWorkshopRepository _workshopRepository;

        public AddUserCommandValidator(IUserRepository userRepository, IWorkshopRepository workshopRepository)
        {
            _userRepository = userRepository;
            _workshopRepository = workshopRepository;

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ và tên là bắt buộc.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email là bắt buộc.")
                .EmailAddress().WithMessage("Định dạng email không hợp lệ.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Số điện thoại là bắt buộc.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu là bắt buộc.")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự.");

            RuleFor(x => x.PasswordConfirmed)
                .Equal(x => x.Password).WithMessage("Mật khẩu xác nhận không khớp.");

            RuleFor(x => x.WorkshopId)
                .NotEmpty().WithMessage("Xưởng làm việc là bắt buộc.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Vai trò là bắt buộc.")
                .Must(role => new[] { "Staff", "QC", "Lead" }.Contains(role))
                .WithMessage("Vai trò không hợp lệ. Chỉ chấp nhận: Staff, QC, Lead.");
        }
    }
}