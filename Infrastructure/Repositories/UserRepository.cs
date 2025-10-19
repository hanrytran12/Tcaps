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

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Id == id);
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
