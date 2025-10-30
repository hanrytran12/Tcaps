using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Productions.Query.GetAllProduction
{
    public class GetAllProductionQueryHandler : IRequestHandler<GetAllProductionQuery, List<ProductionDTO>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IMapper _mapper;

        public GetAllProductionQueryHandler(IProductionRepository productionRepository, IMapper mapper)
        {
            _productionRepository = productionRepository;
            _mapper = mapper;
        }
        public async Task<List<ProductionDTO>> Handle(GetAllProductionQuery request, CancellationToken cancellationToken)
        {
            var productions = await _productionRepository.GetAllAsync();
            var dto = _mapper.Map<List<ProductionDTO>>(productions);
            return dto.ToList();
        }
    }
}
