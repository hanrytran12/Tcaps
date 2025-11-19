using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly AppDbContext _context;

        public AssignmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Assignment assignment)
        {
            await _context.Assignments.AddAsync(assignment);
        }

        public async Task<bool> ExistsAndBelongsToBatchAsync(Guid assignmentId, Guid batchId)
        {
            return await _context.Assignments.AnyAsync(a => a.Id == assignmentId && a.BatchId == batchId);
        }

        public async Task<Assignment?> FindByBatchAndStepOrderAsync(Guid batchId, int stepOrder)
        {
            return await _context.Assignments.FirstOrDefaultAsync(x => x.BatchId == batchId && x.StepOrder == stepOrder);
        }

        public async Task<IEnumerable<Assignment>> GetAllAssignmentsAsync()
        {
            return await _context.Assignments.ToListAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsByWorkshopIdAsync(Guid? workshopId)
        {
            return await _context.Assignments
                .Where(a => a.WorkshopId == workshopId)
                .ToListAsync();
        }

        public async Task<Assignment> GetByIdAsync(Guid id)
        {
            return await _context.Assignments.FindAsync(id);
        }

        public async Task<IEnumerable<Assignment>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Assignments
                .Where(a => ids.Contains(a.WorkshopId))
                .ToListAsync();
        }

        public async Task<bool> HasActiveAssignmentByWorkshopIdAsync(Guid? workshopId)
        {
            return await _context.Assignments.AsNoTracking().AnyAsync(a => a.WorkshopId == workshopId && a.Status == "InProgress");
        }

        public void Update(Assignment assignment)
        {
            _context.Assignments.Update(assignment);
        }
    }
}
