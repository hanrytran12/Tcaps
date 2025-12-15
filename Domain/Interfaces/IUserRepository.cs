using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task<bool> DoesEmailExistAsync(string email);
        Task<bool> DoesPhoneExistAsync(string phone);
        Task<bool> IsEmailTakenByAnotherUserAsync(string email, Guid userId);
        Task<bool> IsPhoneTakenByAnotherUserAsync(string phone, Guid userId);
        Task<User?> GetByRoleAsync(string role);
        Task<User?> GetQCByWorkshopIdAsync(Guid? workshopId);
        Task<User?> FindByEmailOrPhoneAsync(string emailOrPhone);
        Task<Guid> GetWorkshopIdByQCIdAsync(Guid qc_id);
        Task<IEnumerable<User>> GetUsersByWorkshopIdAsync(Guid workshopId);
        Task<IEnumerable<User>> GetAllQCTransportAsync();
        Task<List<User>> GetQcsByWorkshopIdsAsync(List<Guid> workshopId);
        Task<IEnumerable<User>> GetLeadsAsync();
    }
}
