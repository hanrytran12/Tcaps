using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.GetAllQCTransport
{
    public class GetAllQCTransportQueryHandler : IRequestHandler<GetAllQCTransportQuery, List<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllQCTransportQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<List<UserDTO>> Handle(GetAllQCTransportQuery request, CancellationToken cancellationToken)
        {
            var qcTransports = await _userRepository.GetAllQCTransportAsync();
            if (qcTransports == null || !qcTransports.Any())
                return new List<UserDTO>();

            var dto = _mapper.Map<List<UserDTO>>(qcTransports);
            return dto;
        }
    }
}
