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
                .EmailAddress().WithMessage("Định dạng email không hợp lệ.")
                .MustAsync(BeUniqueEmail).WithMessage("Email này đã được sử dụng.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Số điện thoại là bắt buộc.")
                .MustAsync(BeUniquePhone).WithMessage("Số điện thoại này đã được sử dụng.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu là bắt buộc.")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự.");

            RuleFor(x => x.PasswordConfirmed)
                .Equal(x => x.Password).WithMessage("Mật khẩu xác nhận không khớp.");

            RuleFor(x => x.WorkshopId)
                .NotEmpty().WithMessage("Xưởng làm việc là bắt buộc.")
                .MustAsync(WorkshopMustExist).WithMessage("Xưởng làm việc không tồn tại.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Vai trò là bắt buộc.")
                .Must(role => new[] { "Staff", "QC", "Lead" }.Contains(role))
                .WithMessage("Vai trò không hợp lệ. Chỉ chấp nhận: Staff, QC, Lead.");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            var isEmailExist = await _userRepository.DoesEmailExistAsync(email);
            return !isEmailExist;
        }

        private async Task<bool> BeUniquePhone(string phone, CancellationToken cancellationToken)
        {
            var isPhoneExist = await _userRepository.DoesPhoneExistAsync(phone);
            return !isPhoneExist;
        }

        private async Task<bool> WorkshopMustExist(Guid workshopId, CancellationToken cancellationToken)
        {
            return await _workshopRepository.ExistsAsync(workshopId);
        }
    }
}