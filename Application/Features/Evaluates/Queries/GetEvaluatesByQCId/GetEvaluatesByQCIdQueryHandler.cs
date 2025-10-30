using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByQCId
{
    public class GetEvaluatesByQCIdQueryHandler : IRequestHandler<GetEvaluatesByQCIdQuery, List<EvaluateDTO>>
    {
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IComponentDefectRepository _componentDefectRepository;
        private readonly IMapper _mapper;

        public GetEvaluatesByQCIdQueryHandler(IEvaluateRepository evaluateRepository, IComponentDefectRepository componentDefectRepository,
            IMapper mapper)
        {
            _evaluateRepository = evaluateRepository;
            _componentDefectRepository = componentDefectRepository;
            _mapper = mapper;
        }

        public async Task<List<EvaluateDTO>> Handle(GetEvaluatesByQCIdQuery request, CancellationToken cancellationToken)
        {
            var evaluates = await _evaluateRepository.GetByQCIdAsync(request.QC_Id);

            if (!string.IsNullOrEmpty(request.Status))
                evaluates = evaluates
                    .Where(e => e.Status.Equals(request.Status, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            var dto = _mapper.Map<List<EvaluateDTO>>(evaluates);

            foreach ( var item in dto )
            {
                var components = await _componentDefectRepository.GetAllByEvaluateIdAsync(item.Id);
                var componentDtos = _mapper.Map<List<ComponentDefectsDTO>>(components);
                item.Defects = componentDtos;
            }
            return dto; 
        }
    }
}
