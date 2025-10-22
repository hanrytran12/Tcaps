using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<IEnumerable<Assignment>> GetAssignmentsAsync(Guid workshopId);
        Task<Assignment> GetByIdAsync(Guid id);
        Task<IEnumerable<Assignment>> GetByIdsAsync(List<Guid> ids);
        Task<IEnumerable<Assignment>> GetAllAssignmentsAsync();
        Task AddAsync(Assignment assignment);
        void Update(Assignment assignment);
    }
}
