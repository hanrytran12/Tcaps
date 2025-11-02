using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByStaffId
{
    public class GetEvaluatesByStaffIdQueryHandler : IRequestHandler<GetEvaluatesByStaffIdQuery, List<Evaluate>>
    {
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductionRepository _productionRepository;

        public GetEvaluatesByStaffIdQueryHandler(IEvaluateRepository evaluateRepository, IUserRepository userRepository,
            IProductionRepository productionRepository)
        {
            _evaluateRepository = evaluateRepository;
            _userRepository = userRepository;
            _productionRepository = productionRepository;
        }
        public async Task<List<Evaluate>> Handle(GetEvaluatesByStaffIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.StaffId);
            if (user == null)
            {
                throw new Exception($"Không tìm thấy user với ID = {request.StaffId}");
            }

            //lấy ds productions
            var productions = await _productionRepository.GetByUserAsync(user.Id);
            if (!productions.Any())
                throw new Exception("Nhân viên chưa có sản xuất nào cho AssignId này.");

            productions = productions.Where(p => p.AssignId == request.AssignId).ToList();
            //lấy ds productionId
            var productionIds = productions.Select(x => x.Id).ToList();

            //lấy ds evaluate
            var evaluates = await _evaluateRepository.GetByProductionIdsAsync(productionIds);
            return evaluates.ToList();
        }
    }
}
