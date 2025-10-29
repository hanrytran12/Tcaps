using Domain.Interfaces;
using Domain.Primitives;

namespace Domain.Entities
{
    public class User : Entity
    {
        public Guid WorkshopId { get; private set; }
        public string Role { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string? PasswordHash { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string Status { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }


        public User(Guid id, Guid workshopId, string role, string fullName, string email, string passwordHash, string phone)
            : base(id)
        {
            WorkshopId = workshopId;
            Role = role;
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            Phone = phone;
            CreatedAt = DateTime.Now;
            Status = "Active";
        }

        private User() : base(Guid.NewGuid()) { }

        public static User Create(Guid workshopId, string role, string fullName, string email, string passwordHash, string phone)
        {
            return new User(Guid.NewGuid(), workshopId, role, fullName, email, passwordHash, phone);
        }

        public void UpdateDetails(string? role, string? fullName, string? email, string? phone)
        {
            this.Role = !string.IsNullOrEmpty(role) ? role : this.Role;
            this.FullName = !string.IsNullOrEmpty(fullName) ? fullName : this.FullName;
            this.Email = !string.IsNullOrEmpty(email) ? email : this.Email;
            this.Phone = !string.IsNullOrEmpty(phone) ? phone : this.Phone;
        }

        public void MarkAsDeleted()
        {
            this.Status = "Inactive";
        }

        public void UpdateProfile(string fullName, string email, string phone)
        {
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentException("FullName can not be empty");

            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email can not be empty");

            if (string.IsNullOrEmpty(phone))
                throw new ArgumentException("Phone can not be empty");

            FullName = fullName;
            Email = email;
            Phone = phone;
        }

        public void ChangePassword(string currentPasswordHash, string newPasswordHash, IPasswordHasher passwordHasher)
        {
            if (!passwordHasher.Verify(currentPasswordHash, PasswordHash))
                throw new InvalidOperationException("Mật khẩu hiện tại không chính xác.");

            PasswordHash = newPasswordHash;
        }
    }
}
