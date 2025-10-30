using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMaterialUseRepository
    {
        Task AddAsync(MaterialUse materialUse);
    }
}
