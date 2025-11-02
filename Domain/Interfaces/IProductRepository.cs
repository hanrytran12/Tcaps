using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<bool> IsCodeUniqueAsync(string code);
        Task<Product?> GetByCodeAsync(string code);
        Task<int?> GetLastCodeIndexAsync(string prefix);
    }
}
