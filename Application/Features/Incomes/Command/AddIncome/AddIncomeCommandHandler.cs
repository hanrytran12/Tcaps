using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
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
            Console.WriteLine($"AddIncome Handler Context: {_unitOfWork.GetHashCode()}");
            Console.WriteLine($"🔥 Running AddIncomeCommand for Production {request.ProductionId}");

            var staff = await _userRepository.GetByIdAsync(request.UserId);
            if (staff == null)
                return Result<Guid>.Failure("Không tìm thấy nhân viên.");

            var production = await _productionRepository.GetByIdAsync(request.ProductionId);
            if (production == null)
                return Result<Guid>.Failure("Không tìm thấy dữ liệu sản xuất.");

            var assign = await _assignmentRepository.GetByIdAsync(production.AssignId);
            if (assign == null)
                return Result<Guid>.Failure("Không tìm thấy phân công.");

            var evaluate = await _evaluateRepository.GetEvaluateByProductionIdAsync(production.Id);
            if (evaluate == null)
                return Result<Guid>.Failure("Không tìm thấy đánh giá.");

            int quantity = 0;
            if (evaluate.Status == "Passed" || evaluate.Status == "Failed")
            {
                quantity = production.Quantity;
            }
            else if (evaluate.Status == "Rejected")
            {
                quantity = production.Quantity - evaluate.QuantityError;
            }

            var income = Income.Create
            (
                assign.BatchId,
                production.Id,
                staff.Id,
                quantity,
                quantity * assign.UnitPrice
            );
            Console.WriteLine($"✅ Saving Income: Batch={income.BatchId}, TotalPrice={income.TotalPrice}");

            await _incomeRepository.AddAsync(income);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(income.Id);
        }
    }
}
