using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using MediatR;

namespace Application.Features.Users.Queries.GetGroupProgress
{
    public class GetGroupProgressQuery : IRequest<Result<List<GroupProgressDTO>>>
    {
        public Guid UserId { get; set; }
    }
}
