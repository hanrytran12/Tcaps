using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Users.Queries.GetUserProfileById
{
    public class GetUserProfileByIdQuery : IRequest<UserDTO>
    {
        public Guid UserId { get; set; }
    }
}
