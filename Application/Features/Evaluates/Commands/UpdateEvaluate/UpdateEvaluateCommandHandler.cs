using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Response;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Evaluates.Commands.UpdateEvaluate
{
    public class UpdateEvaluateCommandHandler : IRequestHandler<UpdateEvaluateCommand, Result<ResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IComponentDefectRepository _componentDefectRepository;
        private readonly ResponseDTO _responseDTO;

        public UpdateEvaluateCommandHandler(IUnitOfWork unitOfWork, IEvaluateRepository evaluateRepository,
            IComponentDefectRepository componentDefectRepository)
        {
            _unitOfWork = unitOfWork;
            _evaluateRepository = evaluateRepository;
            _componentDefectRepository = componentDefectRepository;
            _responseDTO = new ResponseDTO();
        }

        public async Task<Result<ResponseDTO>> Handle(UpdateEvaluateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var evaluate = await _evaluateRepository.GetByIdAsync(request.Id);
                if (evaluate == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "Không tìm thấy đánh giá.";
                    return Result<ResponseDTO>.Failure(_responseDTO.Message);
                }

                evaluate.Update(request.Note, request.QuantityError, request.Image);
                _evaluateRepository.Update(evaluate);

                foreach (var item in request.Defects)
                {
                    if (!item.Id.HasValue)
                    {
                        var component = Domain.Entities.ComponentDefect.Create(
                            evaluate.Id,
                            item.Description,
                            item.Quantity,
                            item.Status);
                        await _componentDefectRepository.AddAsync(component);
                    }
                    else
                    {
                        var defect = await _componentDefectRepository.GetByIdAsync(item.Id.Value);
                        if (defect != null)
                        {
                            defect.Update(
                                item.Description,
                                item.Quantity,
                                item.Status
                            );
                            _componentDefectRepository.Update(defect);
                        }
                    }
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _responseDTO.StatusCode = 200;
                _responseDTO.Message = "Cập nhật thành công.";
                return Result<ResponseDTO>.Success(_responseDTO);
            }
            catch(Exception ex)
            {
                _responseDTO.StatusCode = 500;
                _responseDTO.Message = ex.Message;
                return Result<ResponseDTO>.Failure(_responseDTO.Message);
            }
        }
    }
}
