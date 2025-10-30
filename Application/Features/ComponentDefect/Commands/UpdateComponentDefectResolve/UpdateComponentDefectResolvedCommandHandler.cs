using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.ComponentDefect.Commands.UpdateComponentDefectResolve
{
    public class UpdateComponentDefectResolvedCommandHandler : IRequestHandler<UpdateComponentDefectResolvedCommand, Result>
    {
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IComponentDefectRepository _componentDefectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateComponentDefectResolvedCommandHandler(IEvaluateRepository evaluateRepository, IComponentDefectRepository componentDefectRepository,
            IUnitOfWork unitOfWork)
        {
            _evaluateRepository = evaluateRepository;
            _componentDefectRepository = componentDefectRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateComponentDefectResolvedCommand request, CancellationToken cancellationToken)
        {
            var evaluate = await _evaluateRepository.GetByIdAsync(request.EvaluateId);
            if (evaluate == null)
                throw new Exception("Không tìm thấy đánh giá.");

            var component = await _componentDefectRepository.GetByIdAsync(request.Id);
            if (component == null)
                throw new Exception("Không tìm thấy thành phần lỗi");

            evaluate.UpdateResolveComponent(component.Id, request.Status);
            //gọi update sẽ update mấy th chill luôn
            _evaluateRepository.Update(evaluate);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
