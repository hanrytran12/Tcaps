using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ProductionService : IProductionService
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResponseDTO _responseDTO;

        public ProductionService(IProductionRepository productionRepository, IMapper mapper,
            IUserRepository userRepository, IEvaluateRepository evaluateRepository,
            INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _productionRepository = productionRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _evaluateRepository = evaluateRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
            _responseDTO = new ResponseDTO();
        }

        public async Task<ResponseDTO> DecreaseQuantityAsync(Guid productionId, CancellationToken cancellationToken)
        {
            try
            {
                var production = await _productionRepository.GetByIdAsync(productionId);
                if (production == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "Production not found.";
                    return _responseDTO;
                }

                production.DecreaseQuantity();
                _productionRepository.Update(production);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _responseDTO.StatusCode = 200;
                _responseDTO.Message = "Success";
            }
            catch (Exception ex)
            {
                _responseDTO.StatusCode = 500;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        public async Task<ResponseDTO> IncreaseQuantityAsync(Guid productionId, CancellationToken cancellationToken)
        {
            try
            {
                var production = await _productionRepository.GetByIdAsync(productionId);
                if (production == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "Production not found.";
                    return _responseDTO;
                }

                production.IncreaseQuantity();
                _productionRepository.Update(production);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _responseDTO.StatusCode = 200;
                _responseDTO.Message = "Success";
            }
            catch (Exception ex)
            {
                _responseDTO.StatusCode = 500;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        public async Task<ResponseDTO> SubmitProductionAsync(ProductionDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(dto.UserId);
                if (user == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User not found.";
                    return _responseDTO;
                }

                var qc = await _userRepository.GetQCByWorkshopIdAsync(user.WorkshopId);
                if (qc == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "QC not found for this workshop.";
                    return _responseDTO;
                }

                var production = await _productionRepository.GetByIdAsync(dto.Id);
                if (production == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "Production not found.";
                    return _responseDTO;
                }

                production.Submit();
                _productionRepository.Update(production);

                var evaluate = new Evaluate(Guid.NewGuid(), production.Id, qc.Id, string.Empty, string.Empty);
                await _evaluateRepository.AddAsync(evaluate);

                var notification = new Notification
                (
                    Guid.NewGuid(),
                    qc.Id,
                    "Yêu cầu đánh giá sản lượng",
                    $"{user.FullName} đã gửi yêu cầu đánh giá sản lượng {dto.Quantity}.",
                    "Yêu cầu đánh giá"
                );
                await _notificationRepository.AddAsync(notification);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _responseDTO.StatusCode = 200;
                _responseDTO.Message = "Sucess";
                _responseDTO.Data = true;
            }
            catch (Exception ex)
            {
                _responseDTO.StatusCode = 500;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        public async Task<ResponseDTO> UpdateQuantityAsync(Guid productionId, int newQuantity, CancellationToken cancellationToken)
        {
            try
            {
                var production = await _productionRepository.GetByIdAsync(productionId);
                if (production == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "Production not found.";
                    return _responseDTO;
                }

                production.SetQuantity(newQuantity);
                _productionRepository.Update(production);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _responseDTO.StatusCode = 200;
                _responseDTO.Message = "Success";
            }
            catch (Exception ex)
            {
                _responseDTO.StatusCode = 500;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }
    }
}
