using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users.Queries.GetUserProfileById
{
    public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, UserDTO>
    {
        private readonly IAppDbContext _context;

        public GetUserProfileByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }
        public async Task<UserDTO> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = await (from user in _context.Users
                             join workshop in _context.Workshop
                             on user.WorkshopId equals workshop.Id
                             where user.Id == request.UserId
                             select new UserDTO
                             {
                                 Id = user.Id,
                                 WorkshopId = workshop.Id,
                                 WorkshopName = workshop.Name,
                                 Role = user.Role,
                                 FullName = user.FullName,
                                 Email = user.Email,
                                 Phone = user.Phone,
                                 Status = user.Status,
                                 CreatedAt = user.CreatedAt
                             }).FirstOrDefaultAsync();
            if (dto == null)
            {
                return new UserDTO();
            }
            return dto;
        }
    }
}
