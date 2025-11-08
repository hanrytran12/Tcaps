using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetUserByWorkshopId
{
    public class GetUserByWorkshopIdQueryHandler : IRequestHandler<GetUserByWorkshopIdQuery, User>
    {
        private readonly IAppDbContext _appDbContext;
        public GetUserByWorkshopIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<User> Handle(GetUserByWorkshopIdQuery request, CancellationToken cancellationToken)
        {
            return await _appDbContext.Users.AsNoTracking().Where(u => u.WorkshopId == request.WorkshopId && u.Role == "QC").FirstOrDefaultAsync(cancellationToken);
        }
    }
}
