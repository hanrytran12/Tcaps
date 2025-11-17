using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<Product?> GetByCodeAsync(string code)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int?> GetLastCodeIndexAsync(string prefix)
        {
            var query = _context.Products
                .Where(p => p.Code.StartsWith(prefix))
                .Select(p => p.Code.Substring(prefix.Length));

            var numberQuery = query.Select(p => int.Parse(p));

            if (!await numberQuery.AnyAsync())
            {
                return null;
            }

            var lastNumberString = await query
                .OrderByDescending(p => p.Length)
                .ThenByDescending(p => p)
                .FirstOrDefaultAsync();

            if (int.TryParse(lastNumberString, out var lastIndex))
            {
                return lastIndex;
            }

            return null;
        }

        public async Task<bool> IsCodeUniqueAsync(string code)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Code == code);
            return (product is null);
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            return !await _context.Products.AsNoTracking().AnyAsync(p => p.Name == name && !p.IsDeleted);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
