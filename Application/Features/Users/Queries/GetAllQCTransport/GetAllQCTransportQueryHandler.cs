using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Queries.GetAllQCTransport
{
    public class GetAllQCTransportQueryHandler : IRequestHandler<GetAllQCTransportQuery, Result<List<UserDTO>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllQCTransportQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<Result<List<UserDTO>>> Handle(GetAllQCTransportQuery request, CancellationToken cancellationToken)
        {
            var qcTransports = await _userRepository.GetAllQCTransportAsync();
            if (qcTransports == null || !qcTransports.Any())
                return Result<List<UserDTO>>.Failure("Không tìm thấy QC vận chuyển nào.");

            var dto = _mapper.Map<List<UserDTO>>(qcTransports);
            return Result<List<UserDTO>>.Success(dto);
        }
    }
}
