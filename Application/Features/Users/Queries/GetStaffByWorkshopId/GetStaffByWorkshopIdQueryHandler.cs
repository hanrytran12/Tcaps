using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetStaffByWorkshopId
{
    public class GetStaffByWorkshopIdQueryHandler : IRequestHandler<GetStaffByWorkshopIdQuery, List<UsersDTO>>
    {
        private readonly IAppDbContext _appDbContext;

        public GetStaffByWorkshopIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<UsersDTO>> Handle(GetStaffByWorkshopIdQuery request, CancellationToken cancellationToken)
        {
            var query = from w in _appDbContext.Workshops
                        join u in _appDbContext.Users on w.Id equals u.WorkshopId
                        where w.Id == request.WorkshopId && (u.Role == "Staff")
                        select new UsersDTO
                        {
                            Id = u.Id,
                            FullName = u.FullName,
                            Email = u.Email,
                            Role = u.Role,
                            WorkshopName = w.Name,
                            CreatedAt = u.CreatedAt,
                            Status = u.Status,
                            Phone = u.Phone
                        };
            return await query.ToListAsync(cancellationToken);
        }
    }
}
