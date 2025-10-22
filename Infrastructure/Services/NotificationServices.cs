using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class NotificationServices : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IMapper _mapper;
        private readonly ResponseDTO _responseDTO;

        public NotificationServices(IUnitOfWork unitOfWork, IUserRepository userRepository, INotificationRepository notificationRepository, IMaterialRepository materialRepository, IBatchRepository batchRepository
            ,IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _materialRepository = materialRepository;
            _batchRepository = batchRepository;
            _mapper = mapper;
            _responseDTO = new ResponseDTO();
        }

        public async Task<ResponseDTO> MarkAsReadAsync(Guid notificationId)
        {
            try
            {
                var notification = await _notificationRepository.GetByIdAsync(notificationId);
                if (notification == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "Không tìm thấy thông báo này.";
                    return _responseDTO;
                }

                await _notificationRepository.MarkAsReadAsync(notificationId);
                await _unitOfWork.SaveChangesAsync();

                _responseDTO.StatusCode = 200;
                _responseDTO.Message = "Success";
            }
            catch(Exception ex)
            {
                _responseDTO.StatusCode = 500;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        public async Task SendBatchCompletionNotificationAsync(Guid batchId, string batchCode)
        {
            var user = await _userRepository.GetByRoleAsync("Admin");
            var title = $"Batch {batchCode} Completed";
            var message = $"Batch {batchCode} with ID {batchId} has been completed.";
            var type = "BatchCompletion";
            var notification = new Notification(Guid.NewGuid(), user.Id, title, message, type);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendMaterialRequestApprovalNotificationAsync(Guid materialId, Guid batchId, decimal quantityRequest)
        {
            var user = await _userRepository.GetByRoleAsync("Lead");
            var batch = await _batchRepository.GetByIdAsync(batchId);
            var material = await _materialRepository.GetByIdAsync(materialId);
            var admin = await _userRepository.GetByRoleAsync("Admin");

            var title = "Material Request Approved";
            var message = $"Lead {user?.FullName} is approve request for {quantityRequest} of material {material?.Name} for batch {batch?.Code}.";
            var type = "MaterialRequestApproval";
            var notification = new Notification(Guid.NewGuid(), admin.Id, title, message, type);

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ResponseDTO> GetNotificationByUserIdAsync(Guid userId)
        {
            try
            {
                var notifications = await _notificationRepository.GetByUserIdAsync(userId);
                if (notifications == null || !notifications.Any())
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "Không có thông báo.";
                    return _responseDTO;
                }

                var dto = _mapper.Map<List<NotificationDTO>>(notifications);
                _responseDTO.StatusCode = 200;
                _responseDTO.Message = "Success";
                _responseDTO.Data = dto;
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
