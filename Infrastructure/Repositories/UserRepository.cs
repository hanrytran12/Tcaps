using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<bool> DoesEmailExistAsync(string email)
        {
            var result = await _context.Users.AnyAsync(x => x.Email == email);
            return result;
        }

        public async Task<bool> DoesPhoneExistAsync(string phone)
        {
            var result = await _context.Users.AnyAsync(x => x.Phone == phone);
            return result;
        }

        public async Task<User?> FindByEmailOrPhoneAsync(string emailOrPhone)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == emailOrPhone || x.Phone == emailOrPhone);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllQCTransportAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "QCTransport" && u.Status == "Active")
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<User?> GetByRoleAsync(string role)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Role == role);
        }

        public async Task<IEnumerable<User>> GetLeadsAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "Lead" && u.Status == "Active")
                .ToListAsync();
        }

        public async Task<User?> GetQCByWorkshopIdAsync(Guid? workshopId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.WorkshopId == workshopId && u.Role == "QC");
        }

        public async Task<List<User>> GetQcsByWorkshopIdsAsync(List<Guid> workshopId)
        {
            return await _context.Users.AsNoTracking().Where(u => u.WorkshopId.HasValue && workshopId.Contains(u.WorkshopId.Value) && (u.Role == "QC" || u.Role == "QCK")).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByWorkshopIdAsync(Guid workshopId)
        {
            return await _context.Users
                .Where(u => u.WorkshopId == workshopId && u.Role == "Staff")
                .ToListAsync();
        }

        public async Task<Guid> GetWorkshopIdByQCIdAsync(Guid qc_id)
        {
            return (Guid)await _context.Users
                .Where(u => u.Id == qc_id)
                .Select(u => u.WorkshopId).FirstOrDefaultAsync();
        }

        public async Task<bool> IsEmailTakenByAnotherUserAsync(string email, Guid userId)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.Id != userId);
        }

        public async Task<bool> IsPhoneTakenByAnotherUserAsync(string phone, Guid userId)
        {
            return await _context.Users.AnyAsync(u => u.Phone == phone && u.Id != userId);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
    }
}
