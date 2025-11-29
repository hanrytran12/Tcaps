using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetAllUser
{
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, List<UsersDTO>>
    {
        //private readonly IUserRepository _repository;
        private readonly IAppDbContext _appDbContext;

        public GetAllUserQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<UsersDTO>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = from u in _appDbContext.Users
                        where u.Role != "Admin"
                        join w in _appDbContext.Workshop on u.WorkshopId equals w.Id into userWorkshops
                        from subW in userWorkshops.DefaultIfEmpty()
                        select new UsersDTO
                        {
                            Id = u.Id,
                            Role = u.Role,
                            FullName = u.FullName,
                            Email = u.Email,
                            Phone = u.Phone,
                            CreatedAt = u.CreatedAt,
                            WorkshopName = subW != null ? subW.Name : string.Empty,
                            Status = u.Status
                        };

            return await users.AsNoTracking().ToListAsync();
        }
    }
}
