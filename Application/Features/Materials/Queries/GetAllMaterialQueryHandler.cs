using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Materials.Queries
{
    public class GetAllMaterialQueryHandler : IRequestHandler<GetAllMaterialQuery, List<MaterialDTO>>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IMapper _mapper;

        public GetAllMaterialQueryHandler(IMaterialRepository materialRepository, IMapper mapper)
        {
            _materialRepository = materialRepository;
            _mapper = mapper;
        }
        public async Task<List<MaterialDTO>> Handle(GetAllMaterialQuery request, CancellationToken cancellationToken)
        {
            var materials = await _materialRepository.GetAllAsync();

            // Nếu không có dữ liệu thì trả về list rỗng
            if (materials == null || !materials.Any())
                return new List<MaterialDTO>();

            // Nếu có MaterialName, thì lọc theo tên
            if (!string.IsNullOrEmpty(request.MaterialName))
            {
                materials = materials
                    .Where(m => m.Name.Contains(request.MaterialName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var dto = _mapper.Map<List<MaterialDTO>>(materials);
            return dto;
        }
    }
}
