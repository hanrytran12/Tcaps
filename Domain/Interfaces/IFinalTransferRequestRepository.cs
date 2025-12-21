using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IFinalTransferRequestRepository
    {
        Task<IEnumerable<FinalTransferRequest>> GetAllAsync();
        Task<FinalTransferRequest> GetByIdAsync(Guid id);
        Task AddAsync(FinalTransferRequest request);
        void UpdateAsync(FinalTransferRequest finalTransferRequest);
    }
}
