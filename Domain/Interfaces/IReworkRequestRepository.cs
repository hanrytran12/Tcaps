using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IReworkRequestRepository
    {
        Task AddReworkRequestAsync(ReworkRequest reworkRequest);
    }
}
