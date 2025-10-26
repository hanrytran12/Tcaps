using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Evaluates.Queries.GetAllEvaluate
{
    public class GetAllEvaluateQueryHandler : IRequestHandler<GetAllEvaluateQuery, List<EvaluateDTO>>
    {
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IMapper _mapper;
        private readonly IComponentDefectRepository _componentDefectRepository;

        public GetAllEvaluateQueryHandler(IEvaluateRepository evaluateRepository, IMapper mapper,
            IComponentDefectRepository componentDefectRepository)
        {
            _evaluateRepository = evaluateRepository;
            _mapper = mapper;
            _componentDefectRepository = componentDefectRepository;
        }

        public async Task<List<EvaluateDTO>> Handle(GetAllEvaluateQuery request, CancellationToken cancellationToken)
        {
            var evaluates = await _evaluateRepository.GetAllAsync();
            var dto = _mapper.Map<List<EvaluateDTO>>(evaluates);
            foreach ( var item in dto )
            {
                var components = await _componentDefectRepository.GetAllByEvaluateIdAsync(item.Id);
                var componentDto = _mapper.Map<List<ComponentDefectsDTO>>(components);
                item.Defects = componentDto;
            }
            
            return dto;
        }
    }
}
