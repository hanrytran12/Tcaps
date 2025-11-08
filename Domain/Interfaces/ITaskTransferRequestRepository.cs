using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITaskTransferRequestRepository
    {
        Task<IEnumerable<TaskTransferRequest>> GetAllAsync();
        Task<TaskTransferRequest?> GetByIdAsync(Guid id);
        Task<IEnumerable<TaskTransferRequest>> GetByQcTransportIdAsync(Guid qcTransportId);
        Task AddAsync(TaskTransferRequest request);
        void Update(TaskTransferRequest request);
    }
}
