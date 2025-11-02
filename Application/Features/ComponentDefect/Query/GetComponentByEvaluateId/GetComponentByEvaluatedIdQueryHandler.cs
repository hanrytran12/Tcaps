using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.ComponentDefect.Query.GetComponentByEvaluateId
{
    public class GetComponentByEvaluatedIdQueryHandler : IRequestHandler<GetComponentByEvaluatedIdQuery, List<Domain.Entities.ComponentDefect>>
    {
        private readonly IComponentDefectRepository _componentDefectRepository;

        public GetComponentByEvaluatedIdQueryHandler(IComponentDefectRepository componentDefectRepository)
        {
            _componentDefectRepository = componentDefectRepository;
        }
        public async Task<List<Domain.Entities.ComponentDefect>> Handle(GetComponentByEvaluatedIdQuery request, CancellationToken cancellationToken)
        {
            var components = await _componentDefectRepository.GetAllByEvaluateIdAsync(request.EvaluatedId);
            return components.ToList();
        }
    }
}
