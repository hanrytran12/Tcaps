using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetUserByWorkshopId
{
    public class GetUserByWorkshopIdQueryHandler : IRequestHandler<GetUserByWorkshopIdQuery, UserDTO>
    {
        private readonly IAppDbContext _appDbContext;

        public GetUserByWorkshopIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<UserDTO> Handle(GetUserByWorkshopIdQuery request, CancellationToken cancellationToken)
        {
            var user = await (from u in _appDbContext.Users
                              join workshop in _appDbContext.Workshops
                                  on u.WorkshopId equals workshop.Id into userWorkshops
                              from subWorkshop in userWorkshops.DefaultIfEmpty()
                              where u.WorkshopId == request.WorkshopId && u.Role == "QC"
                              select new UserDTO
                              {
                                  Id = u.Id,
                                  WorkshopId = subWorkshop != null ? subWorkshop.Id : Guid.Empty,
                                  WorkshopName = subWorkshop != null ? subWorkshop.Name : string.Empty,
                                  Role = u.Role,
                                  FullName = u.FullName,
                                  Email = u.Email,
                                  Phone = u.Phone,
                                  Status = u.Status,
                                  CreatedAt = u.CreatedAt
                              })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("User not found for the given WorkshopId.");
            }

            return user;
        }
    }
}
