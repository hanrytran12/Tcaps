using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly IAppDbContext _appDbContext;

        public GetUserByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = from u in _appDbContext.Users
                       where u.Id == request.UserId && u.Status == "Active"
                       join w in _appDbContext.Workshops on u.WorkshopId equals w.Id into userWorkshop
                       from subW in userWorkshop.DefaultIfEmpty()
                       select new UserDTO
                       {
                           Id = u.Id,
                           Role = u.Role,
                           FullName = u.FullName,
                           WorkshopId = subW != null ? subW.Id : Guid.Empty,
                           WorkshopName = subW != null ? subW.Name : string.Empty,
                           Email = u.Email,
                           Phone = u.Phone,
                           CreatedAt = DateTime.Now,
                           Status = u.Status,
                       };
            return await user.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
