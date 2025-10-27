using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;

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
        private readonly IProductionRepository _productionRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly ResponseDTO _responseDTO;

        public NotificationServices(IUnitOfWork unitOfWork, IUserRepository userRepository, INotificationRepository notificationRepository, IMaterialRepository materialRepository, IBatchRepository batchRepository
            , IMapper mapper, IProductionRepository productionRepository, IIncomeRepository incomeRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _materialRepository = materialRepository;
            _batchRepository = batchRepository;
            _mapper = mapper;
            _productionRepository = productionRepository;
            _incomeRepository = incomeRepository;
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
            catch (Exception ex)
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

        public async Task<ResponseDTO> GetNotificationByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            try
            {
                //var notifications = await _notificationRepository.GetByUserIdAsync(userId);
                //lấy dữ liệu có phân trang
                var notifications = await _notificationRepository.GetPagedAsync(
                    filter: n => n.UserId == userId,
                    orderBy: q => q.OrderByDescending(n => n.CreatedAt),
                    pageSize: pageSize,
                    pageNumber: pageNumber);

                if (notifications == null || !notifications.Items.Any())
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "Không có thông báo.";
                    return _responseDTO;
                }

                var dto = _mapper.Map<List<NotificationDTO>>(notifications.Items);

                _responseDTO.StatusCode = 200;
                _responseDTO.Message = "Success";
                _responseDTO.Data = new
                {
                    Items = dto,
                    notifications.TotalCount,
                    notifications.TotalPages,
                    notifications.PageSize,
                    notifications.CurrentPage
                };
            }
            catch (Exception ex)
            {
                _responseDTO.StatusCode = 500;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        public async Task<ResponseDTO> CountNotificationAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User not found.";
                    return _responseDTO;
                }

                var count = await _notificationRepository.CountNotificationAsync(userId);
                if (count == 0)
                {
                    _responseDTO.StatusCode = 200;
                    _responseDTO.Message = "Bạn không có thông báo.";
                    _responseDTO.Data = count;
                    return _responseDTO;
                }

                _responseDTO.StatusCode = 200;
                _responseDTO.Message = "Success";
                _responseDTO.Data = count;
            }
            catch(Exception ex)
            {
                _responseDTO.StatusCode = 500;
                _responseDTO.Message = ex.Message;
            }
            return _responseDTO;
        }

        public async Task SendStockUpdateNotificationToAdminAsync(string name, int newStockQuantity, int stockChange)
        {
            var admin = await _userRepository.GetByRoleAsync("Admin");
            var title = "Material Stock Updated";
            var message = $"Material {name} stock has been updated. New stock quantity: {newStockQuantity} (Change: {stockChange}).";
            var type = "MaterialStockUpdate";
            var notification = new Notification(Guid.NewGuid(), admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendEvaluateFixErrorNotificationAsync(Guid evaluateId, Guid productionId, Guid userId, int quantityError, string note, string status)
        {
            var staff = await _productionRepository.GetStaffByProductionIdAsync(productionId);
            var title = "";
            var message = "";
            var type = "";

            if (status == "Pass")
            {
                title = "Không có sản phẩm lỗi.";
                message = $"Sản phẩm của {staff.FullName} không có sản phẩm lỗi. Ghi chú: {note}.";
                type = "EvaluatePass";
            }
            else if (status == "Fail")
            {
                if (quantityError > 0)
                {
                    var production = await _productionRepository.GetByIdAsync(productionId);
                    if (production != null)
                    {
                        production.ReduceQuantity(quantityError);
                        _productionRepository.Update(production);
                    }
                }

                if (quantityError > 0)
                {
                    var income = await _incomeRepository.GetByProductionIdAsync(productionId);
                    if (income != null)
                    {
                        income.ReduceQuantity(quantityError);
                        _incomeRepository.Update(income);
                    }
                }

                title = "Báo lỗi sản phẩm";
                message = $"Sản phẩm của {staff.FullName} có {quantityError} sản phẩm lỗi. Ghi chú: {note}.";
                type = "EvaluateFail";
            }
            else
            {
                if (quantityError > 0)
                {
                    var production = await _productionRepository.GetByIdAsync(productionId);
                    if (production != null)
                    {
                        production.ReduceQuantity(quantityError);
                        _productionRepository.Update(production);
                    }
                }

                if (quantityError > 0)
                {
                    var income = await _incomeRepository.GetByProductionIdAsync(productionId);
                    if (income != null)
                    {
                        income.ReduceQuantity(quantityError);
                        _incomeRepository.Update(income);
                    }
                }

                title = "Báo lỗi sản phẩm";
                message = $"Sản phẩm của {staff.FullName} có {quantityError} sản phẩm lỗi và không thể sữa chữa. Ghi chú: {note}.";
                type = "EvaluateReject";
            }

            var notification = new Notification(Guid.NewGuid(), staff.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
