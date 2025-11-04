using System.Data;
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
        private readonly IWorkshopRepository _workshopRepository;
        private readonly IComponentDefectRepository _componentDefectRepository;
        private readonly IMapper _mapper;
        private readonly IProductionRepository _productionRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly ResponseDTO _responseDTO;

        public NotificationServices(IUnitOfWork unitOfWork, IUserRepository userRepository, INotificationRepository notificationRepository, IMaterialRepository materialRepository, IBatchRepository batchRepository
            , IMapper mapper, IProductionRepository productionRepository, IIncomeRepository incomeRepository,
            IEvaluateRepository evaluateRepository, IWorkshopRepository workshopRepository, IComponentDefectRepository componentDefectRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _materialRepository = materialRepository;
            _batchRepository = batchRepository;
            _workshopRepository = workshopRepository;
            _componentDefectRepository = componentDefectRepository;
            _mapper = mapper;
            _productionRepository = productionRepository;
            _incomeRepository = incomeRepository;
            _evaluateRepository = evaluateRepository;
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
            catch (Exception ex)
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
        }


        public async Task SendEvaluateFixErrorNotificationAsync(Guid evaluateId, Guid productionId, Guid userId, int quantityError, string note, string status)
        {
            var staff = await _productionRepository.GetStaffByProductionIdAsync(productionId);
            var production = await _productionRepository.GetByIdAsync(productionId);
            var title = "";
            var message = "";
            var type = "";

            if (status == "Passed")
            {
                title = "Không có sản phẩm lỗi.";
                message = $"Sản phẩm của {staff?.FullName} không có sản phẩm lỗi. Ghi chú: {note}.";
                type = "EvaluatePass";
                production?.MarkAsCompleted();
                _productionRepository.Update(production);
            }
            else if (status == "Failed")
            {
                title = "Báo lỗi sản phẩm";
                message = $"Sản phẩm của {staff?.FullName} có {quantityError} sản phẩm lỗi. Ghi chú: {note}.";
                type = "EvaluateFail";
                production?.Rework();
                _productionRepository.Update(production);
            }
            else
            {
                title = "Báo lỗi sản phẩm";
                message = $"Sản phẩm của {staff?.FullName} có {quantityError} sản phẩm lỗi và không thể sữa chữa. Ghi chú: {note}.";
                type = "EvaluateReject";
                production?.CompleteWithLoss();
                _productionRepository.Update(production);
            }

            var notification = new Notification(Guid.NewGuid(), staff.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendSubmitProductionNotification(Guid assignId, Guid userId, int quantity)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var qc = await _userRepository.GetQCByWorkshopIdAsync(user.WorkshopId);

            var title = "Nộp sản phẩm";
            var message = $"Nhân viên {user.FullName} nộp {quantity} sản phẩm để QC kiểm tra.";
            var type = "SubmitProduction";
            var notification = new Notification(Guid.NewGuid(), qc.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendAssignmentAddNotificationToQcAsync(string batchCode, Guid workshopId, DateOnly expectedDeliveryDate)
        {
            var qc = await _userRepository.GetQCByWorkshopIdAsync(workshopId);
            var workshop = await _workshopRepository.GetByIdAsync(workshopId);
            var title = "Công việc mới được giao";
            var message = $"Một lô hàng mới, mã lô {batchCode}, vừa được phân công cho xưởng của bạn {workshop?.Name}. Dự kiến giao nguyên liệu vào ngày {expectedDeliveryDate}";
            var type = "NEW_ASSIGNMENT";

            var notification = new Notification(Guid.NewGuid(), qc.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
        }

        public async Task SendComponentResolvedNotification(Guid componentId, Guid evaluateId, int quantity, string status)
        {
            var evaluate = await _evaluateRepository.GetByIdAsync(evaluateId);

            var qc = await _userRepository.GetByIdAsync(evaluate.UserId.Value);

            var production = await _productionRepository.GetByIdAsync(evaluate.ProductionId);

            var user = await _userRepository.GetByIdAsync(production.UserId);

            var title = "Nộp sản phẩm đã sửa lỗi.";
            var message = $"Nhân viên {user.FullName} nộp {quantity} sản phẩm đã sửa chữa để QC kiểm tra.";
            var type = "ResolveProduction";
            var notification = new Notification(Guid.NewGuid(), qc.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendComponentConfirmNotification(Guid componentId, Guid evaluateId, int quantity, string status)
        {
            var evaluate = await _evaluateRepository.GetByIdAsync(evaluateId);

            var qc = await _userRepository.GetByIdAsync(evaluate.UserId.Value);

            var production = await _productionRepository.GetByIdAsync(evaluate.ProductionId);

            var user = await _userRepository.GetByIdAsync(production.UserId);

            var componentDefects = await _componentDefectRepository.GetAllByEvaluateIdAsync(evaluateId);
            if (componentDefects.All(a => a.Status == "Confirmed"))
            {
                production.MarkAsCompleted();
                _productionRepository.Update(production);
            }
            var title = "Chấp nhận sản phẩm đã sửa lỗi thành công.";
            var message = $"QC {qc.FullName} chấp nhận {quantity} sản phẩm đã sửa chữa thành công.";
            var type = "ConfirmProduction";
            var notification = new Notification(Guid.NewGuid(), user.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendStockUpdateNotificationToLeadAsync(string name, int newStockQuantity, int stockChange)
        {
            var lead = await _userRepository.GetByRoleAsync("Lead");
            var title = "Material Stock Updated";
            var message = $"Material {name} stock has been updated. New stock quantity: {newStockQuantity} (Change: {stockChange}).";
            var type = "MaterialStockUpdate";
            var notification = new Notification(Guid.NewGuid(), lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
        }

        public async Task CreateStockUpdateNotificationForRoleAsync(string role, string materialName, int newStock, int change)
        {
            var users = await _userRepository.GetByRoleAsync(role);
            if (users is null) return;

            var title = "Cập nhật Tồn kho Nguyên vật liệu";
            var message = $"Tồn kho của '{materialName}' đã thay đổi. Số lượng mới: {newStock} (thay đổi: {change}).";
            var type = "MaterialStockUpdate";

            var notification = Notification.Create(users.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
        }

        public async Task SendMaterialWorkshopConfirmNotificationAsync(Guid workshopId, int quantitySend, int quantityReceive, string name)
        {
            var lead = await _userRepository.GetByRoleAsync("Lead");
            var admin = await _userRepository.GetByRoleAsync("Admin");
            if (lead is null || admin is null) return;

            var title = "Chấp nhận đơn hàng";
            var message = "";
            var type = "ConfirmMaterialWorkshop";

            var notification = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
        }
    }
}
