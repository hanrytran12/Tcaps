using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.ComponentDefect.Commands.RejectComponentFromQC
{
    public class RejectComponentFromQCCommandHandler : IRequestHandler<RejectComponentFromQCCommand, Result>
    {
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RejectComponentFromQCCommandHandler(IEvaluateRepository evaluateRepository, IUnitOfWork unitOfWork)
        {
            _evaluateRepository = evaluateRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(RejectComponentFromQCCommand request, CancellationToken cancellationToken)
        {
            var evaluate = await _evaluateRepository.GetByIdAsync(request.EvaluateId);
            if (evaluate is null)
            {
                throw new NotFoundException("Không tìm thấy đánh giá (Evaluate).");
            }

            try
            {
                evaluate.RejectComponent(request.Id, request.QuantityReject);
                _evaluateRepository.Update(evaluate);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
