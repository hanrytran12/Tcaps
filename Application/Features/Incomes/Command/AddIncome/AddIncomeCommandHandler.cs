using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Incomes.Command.AddIncome
{
    public class AddIncomeCommandHandler : IRequestHandler<AddIncomeCommand, Result<Guid>>
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IProductionRepository _productionRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IComponentDefectRepository _componentDefectRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public AddIncomeCommandHandler(IIncomeRepository incomeRepository, IProductionRepository productionRepository,
            IAssignmentRepository assignmentRepository, IEvaluateRepository evaluateRepository,
            IComponentDefectRepository componentDefectRepository, IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            _incomeRepository = incomeRepository;
            _productionRepository = productionRepository;
            _assignmentRepository = assignmentRepository;
            _evaluateRepository = evaluateRepository;
            _componentDefectRepository = componentDefectRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async Task<Result<Guid>> Handle(AddIncomeCommand request, CancellationToken cancellationToken)
        {
            var staff = await _userRepository.GetByIdAsync(request.UserId);
            if (staff == null)
                throw new NotFoundException("Không tìm thấy nhân viên.");

            var production = await _productionRepository.GetByIdAsync(request.ProductionId);
            if (production == null)
                throw new NotFoundException("Không tìm thấy dữ liệu sản xuất.");

            var assign = await _assignmentRepository.GetByIdAsync(production.AssignId);
            if (assign == null)
                throw new NotFoundException("Không tìm thấy phân công.");

            var evaluate = await _evaluateRepository.GetEvaluateByProductionIdAsync(production.Id);
            if (evaluate == null)
                throw new NotFoundException("Không tìm thấy đánh giá.");

            //lấy ds componentDefect
            var componentDefects = await _componentDefectRepository.GetAllByEvaluateIdAsync(evaluate.Id);
            int unfixableSum = componentDefects
                .Where(cd => cd.Status.Equals("Unfixable", StringComparison.OrdinalIgnoreCase))
                .Sum(cd => cd.Quantity);

            int quantity = 0;
            if (evaluate.Status == "Passed")
            {
                quantity = production.QuantityReceive;
            }
            else if (evaluate.Status == "Rejected")
            {
                quantity = production.QuantityReceive - evaluate.QuantityError;
            }
            else if (evaluate.Status == "Failed")
            {
                if (unfixableSum > 0)
                    quantity = production.QuantityReceive - unfixableSum;
                else
                    quantity = production.QuantityReceive;
            }

            var income = Income.Create
            (
                assign.BatchId,
                production.Id,
                staff.Id,
                quantity,
                quantity * assign.UnitPrice
            );

            await _incomeRepository.AddAsync(income);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(income.Id);
        }
    }
}
