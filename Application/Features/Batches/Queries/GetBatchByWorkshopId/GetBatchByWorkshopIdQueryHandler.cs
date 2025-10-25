using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Batches.Queries.GetBatchByWorkshopId
{
    public class GetBatchByWorkshopIdQueryHandler : IRequestHandler<GetBatchByWorkshopIdQuery, List<Batch>>
    {
        private readonly IAppDbContext _context;
        public GetBatchByWorkshopIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Batch>> Handle(GetBatchByWorkshopIdQuery request, CancellationToken cancellationToken)
        {
            //var originalList = new List<Batch>();
            //var batches = await _batchRepository.GetAllWithAssignmentsAsync();
            //foreach (var batch in batches)
            //{
            //    var assignments = batch.Assignments.FirstOrDefault(x => x.WorkshopId == request.WorkshopId);
            //    if (assignments is not null)
            //    {
            //        originalList.Add(batch);
            //    }
            //}

            //var filterList = originalList.Where(x => x.Status == request.Status);
            //return (request.Status is null ? originalList.ToList() : filterList.ToList());

            var query = _context.Batches.AsQueryable();

            query = query.Where(batch => batch.Assignments.Any(assigment => assigment.WorkshopId == request.WorkshopId));

            if (!string.IsNullOrEmpty(request.Status))
            {
                query = query.Where(batch => batch.Status == request.Status);
            }

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
