using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.GetAllLead
{
    public class GetAllLeadQueryHandler : IRequestHandler<GetAllLeadQuery, List<UserDTO>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllLeadQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<List<UserDTO>> Handle(GetAllLeadQuery request, CancellationToken cancellationToken)
        {
            var leads = await _userRepository.GetLeadsAsync();
            if (leads == null || !leads.Any())
            {
                return new List<UserDTO>();
            }

            var leadDTOs = leads.Select(lead => new UserDTO
            {
                Id = lead.Id,
                WorkshopId = lead.WorkshopId ?? Guid.Empty,
                Role = lead.Role,
                FullName = lead.FullName,
                Email = lead.Email,
                Phone = lead.Phone,
                Status = lead.Status,
                CreatedAt = lead.CreatedAt,
            }).ToList();

            return leadDTOs;
        }
    }
}
