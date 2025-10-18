using Domain.Primitives;

namespace Domain.Entities
{
    public class User : Entity
    {
        public Guid? WorkshopId { get; private set; }
        public string Role { get; private set; } = string.Empty;
        public string FullName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string Status { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }


        public User(Guid id, Guid? workshopId, string role, string fullName, string email, string passwordHash, string phone)
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

        public void ChangePassword(string currentPasswordHash, string newPasswordHash)
        {
            if (PasswordHash != currentPasswordHash)
                throw new InvalidOperationException("Mật khẩu hiện tại không chính xác.");

            PasswordHash = newPasswordHash;
        }
    }
}
