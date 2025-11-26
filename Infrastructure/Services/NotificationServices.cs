using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Data;

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
        private readonly IMaterialRequestRepository _materialRequestRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;
        private readonly IProductionRepository _productionRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly ResponseDTO _responseDTO;

        public NotificationServices(IUnitOfWork unitOfWork, IUserRepository userRepository, INotificationRepository notificationRepository, IMaterialRepository materialRepository, IBatchRepository batchRepository
            , IMapper mapper, IProductionRepository productionRepository, IIncomeRepository incomeRepository,
            IEvaluateRepository evaluateRepository, IWorkshopRepository workshopRepository, IComponentDefectRepository componentDefectRepository,
            IMaterialRequestRepository materialRequestRepository, IAssignmentRepository assignmentRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _notificationRepository = notificationRepository;
            _materialRepository = materialRepository;
            _batchRepository = batchRepository;
            _workshopRepository = workshopRepository;
            _componentDefectRepository = componentDefectRepository;
            _materialRequestRepository = materialRequestRepository;
            _assignmentRepository = assignmentRepository;
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

        public async Task SendMaterialRequestApprovalNotificationAsync(Guid materialId, Guid qcId, Guid batchId, decimal quantityRequest)
        {
            var user = await _userRepository.GetByRoleAsync("Lead");
            var qc = await _userRepository.GetByIdAsync(qcId);
            var batch = await _batchRepository.GetByIdAsync(batchId);
            var material = await _materialRepository.GetByIdAsync(materialId);
            var admin = await _userRepository.GetByRoleAsync("Admin");

            var title = "Material Request Approved";
            var message = $"Lead {user?.FullName} is approve request for {quantityRequest} of material {material?.Name} for batch {batch?.Code}.";
            var type = "MaterialRequest";
            var notificationAdmin = new Notification(Guid.NewGuid(), admin.Id, title, message, type);

            await _notificationRepository.AddAsync(notificationAdmin);

            var notificationQC = new Notification(Guid.NewGuid(), qc.Id, title, message, type);

            await _notificationRepository.AddAsync(notificationQC);
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


        public async Task SendEvaluateFixErrorNotificationAsync(Guid evaluateId, Guid productionId, Guid userId, int quantityError, int quantitySuccess, string note, string status)
        {
            var staff = await _productionRepository.GetStaffByProductionIdAsync(productionId);
            var production = await _productionRepository.GetByIdAsync(productionId);
            var evaluate = await _evaluateRepository.GetByIdAsync(evaluateId);
            var title = "";
            var message = "";
            var type = "";

            if (status == "Passed")
            {
                title = "Không có sản phẩm lỗi.";
                message = $"Sản phẩm của {staff?.FullName} không có sản phẩm lỗi. Ghi chú: {note}.";
                type = "Evaluate";
                production?.MarkAsCompleted();
                _productionRepository.Update(production);
            }
            else if (status == "Failed")
            {
                title = "Báo lỗi sản phẩm";
                message = $"Sản phẩm của {staff?.FullName} có {quantitySuccess} sản phẩm đạt và {quantityError} sản phẩm lỗi. Ghi chú: {note}.";
                type = "Evaluate";
                production?.Rework();
                _productionRepository.Update(production);
            }
            else
            {
                title = "Báo lỗi sản phẩm";
                message = $"Sản phẩm của {staff?.FullName} có {quantitySuccess} sản phẩm đạt và {quantityError} sản phẩm lỗi và không thể sữa chữa. Ghi chú: {note}.";
                type = "Evaluate";
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
            var type = "Production";
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
            var type = "Production";
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
            bool allConfirmed = componentDefects.All(x => x.Status == "Confirmed" || x.Status == "Unfixable");
            bool hasUnfixable = componentDefects.Any(x => x.Status == "Unfixable");

            if (allConfirmed)
            {
                if (hasUnfixable)
                    production.CompleteWithLoss();
                else
                    production.MarkAsCompleted();

                _productionRepository.Update(production);
            }

            var title = "Chấp nhận sản phẩm đã sửa lỗi thành công.";
            var message = $"QC {qc.FullName} chấp nhận {quantity} sản phẩm đã sửa chữa thành công.";
            var type = "Production";
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
            var type = "MATERIAL_STOCK_UPDATE";

            var notification = Notification.Create(users.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
        }

        public async Task SendMaterialWorkshopConfirmNotificationAsync(Guid workshopId, int quantitySend, int quantityReceive)
        {
            var lead = await _userRepository.GetByRoleAsync("Lead");
            var admin = await _userRepository.GetByRoleAsync("Admin");
            if (lead is null || admin is null) return;

            var title = "Chấp nhận đơn hàng";
            var message = $"Đã xác nhận đơn gửi {quantitySend} vật liệu và nhận {quantityReceive}.";
            var type = "MaterialWorkshop";

            var notificationLead = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationLead);

            var notificationAdmin = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationAdmin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendCreateTaskTransferRequestNotificationAsync(Guid batchId, Guid workshopId, Guid qcTransportId, string note)
        {
            var admin = await _userRepository.GetByRoleAsync("Admin");
            var qc = await _userRepository.GetByIdAsync(qcTransportId);
            if (admin is null) return;

            var title = "Yêu cầu chuyển giao công việc cho QC vận chuyển";
            var message = $"Lead vừa tạo yêu cầu chuyển giao cho lô {batchId} tại xưởng {workshopId} cho QC tên {qc.FullName}. Ghi chú: {note}";
            var type = "TaskTransferRequest";

            var notification = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendApproveTaskTransferRequestNotificationAsync(Guid taskTransferRequestId, Guid qcTransportId)
        {
            var qcTransport = await _userRepository.GetByIdAsync(qcTransportId);
            if (qcTransport is null) return;

            var title = "Yêu cầu chuyển giao đã được duyệt";
            var message = $"Yêu cầu chuyển giao #{taskTransferRequestId} của bạn đã được duyệt. Vui lòng kiểm tra để tiến hành vận chuyển.";
            var type = "TaskTransferRequest";

            var notification = Notification.Create(qcTransport.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendCreateMaterialRequestNotificationAsync(Guid qcId, Guid batchId, Guid assignId)
        {
            var lead = await _userRepository.GetByRoleAsync("Lead");
            var qc = await _userRepository.GetByIdAsync(qcId);
            if (lead is null || qc is null) return;

            var batch = await _batchRepository.GetByIdAsync(batchId);
            var workshop = await _workshopRepository.GetByIdAsync(qc.WorkshopId);

            var title = "Yêu cầu cung cấp thêm vật liệu";
            var message = $"Yêu cầu cung cấp thêm vật liệu cho lô hàng {batch.Code} tại xưởng {workshop.Name}.";
            var type = "MaterialRequest";

            var notification = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AdminAssignForQCTransportToTransferMaterialSupplyNotificationAsync(Guid qcTransportId, Guid requestId, Guid materialId, int quantity)
        {
            var qcTransport = await _userRepository.GetByIdAsync(qcTransportId);

            var request = await _materialRequestRepository.GetByIdAsync(requestId);

            var batch = await _batchRepository.GetByIdAsync(request.BatchId);

            var qc = await _userRepository.GetByIdAsync(request.UserId);

            var workshop = await _workshopRepository.GetByIdAsync(qc.WorkshopId);

            var material = await _materialRepository.GetByIdAsync(materialId);

            var title = "Admin phân công đi giao NVL";
            var message = $"Admin phân công cho {qcTransport.FullName} cung cấp thêm {quantity} {material.Unit} vật liệu **{material.Name}** cho lô hàng **{batch.Code}** tại xưởng **{workshop.Name}**.";
            var type = "MaterialSupply";

            var notification = Notification.Create(qcTransport.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendAddMaterialSupplyForQcWorkshopNotification(Guid qcworkshopId, Guid requestId, Guid materialId, int quantity)
        {
            var qc = await _userRepository.GetByIdAsync(qcworkshopId);

            var request = await _materialRequestRepository.GetByIdAsync(requestId);

            var batch = await _batchRepository.GetByIdAsync(request.BatchId);

            var workshop = await _workshopRepository.GetByIdAsync(qc.WorkshopId);

            var material = await _materialRepository.GetByIdAsync(materialId);

            var title = "Lead gửi NVL";
            var message = $"Lead sẽ cung cấp thêm {quantity} {material.Unit} vật liệu **{material.Name}** cho lô hàng **{batch.Code}** tại xưởng **{workshop.Name}**.";
            var type = "MaterialSupply";

            var notification = Notification.Create(qc.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendCompletedMaterialSupplyNotificationAsync(Guid supplyId, Guid materialId, int quantity)
        {
            var material = await _materialRepository.GetByIdAsync(materialId);
            var lead = await _userRepository.GetByRoleAsync("Lead");

            var title = "Hoàn tất cung cấp vật liệu";
            var message = $"QC đã nhận đủ {quantity} {material.Unit} vật liệu **{material.Name}** của đơn cung cấp {supplyId}.";
            var type = "MaterialSupply";

            var notification = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendQCTransportApproveMaterialSupplyNotificationAsync(Guid qcTransportId, Guid materialSupplyId)
        {
            var qcTransport = await _userRepository.GetByIdAsync(qcTransportId);

            var lead = await _userRepository.GetByRoleAsync("Lead");
            var admin = await _userRepository.GetByRoleAsync("Admin");

            var title = "QC vận chuyển tiếp nhận";
            var message = $"QC vận chuyển {qcTransport.FullName} đã tiếp nhận đơn {materialSupplyId} cung cấp NVL.";
            var type = "MaterialSupply";

            var notificationLead = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationLead);

            var notificationAdmin = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationAdmin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendQCTransportReceptionAssignmentTransferNotificationAsync(Guid qcTransportId, Guid assignTransferRequestId, Guid assignId)
        {
            var qcTransport = await _userRepository.GetByIdAsync(qcTransportId);

            var lead = await _userRepository.GetByRoleAsync("Lead");
            var admin = await _userRepository.GetByRoleAsync("Admin");

            var assignment = await _assignmentRepository.GetByIdAsync(assignId);

            var workshop = await _workshopRepository.GetByIdAsync(assignment.WorkshopId);

            var title = "QC vận chuyển tiếp nhận";
            var message = $"QC vận chuyển {qcTransport.FullName} đã tiếp nhận đơn {assignTransferRequestId} để kiểm tra yêu cầu tại xưởng {workshop.Name}.";
            var type = "AssignmentTransferRequest";

            var notificationLead = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationLead);

            var notificationAdmin = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationAdmin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendQCTransportReceptionMaterialRequestNotificationAsync(Guid qcTransportId, Guid materialRequestId, Guid assignId)
        {
            var qcTransport = await _userRepository.GetByIdAsync(qcTransportId);

            var lead = await _userRepository.GetByRoleAsync("Lead");
            var admin = await _userRepository.GetByRoleAsync("Admin");

            var assignment = await _assignmentRepository.GetByIdAsync(assignId);

            var workshop = await _workshopRepository.GetByIdAsync(assignment.WorkshopId);

            var title = "QC vận chuyển tiếp nhận";
            var message = $"QC vận chuyển {qcTransport.FullName} đã tiếp nhận đơn {materialRequestId} để kiểm tra yêu cầu tại xưởng {workshop.Name}.";
            var type = "AssignmentTransferRequest";

            var notificationLead = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationLead);

            var notificationAdmin = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationAdmin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendConfirmRequestFromLeadNotificationAsync(Guid materialRequestId, Guid qcId)
        {
            var qc = await _userRepository.GetByIdAsync(qcId);

            var workshop = await _workshopRepository.GetByIdAsync(qc.WorkshopId);

            var title = "Lead chấp nhận cung cấp NVL";
            var message = $"Lead đã chấp nhận yêu cầu {materialRequestId} cung cấp NVL cho {qc.FullName} tại xưởng {workshop.Name}.";
            var type = "MaterialSupply";

            var notification = Notification.Create(qc.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddBatchNotificationAsync(string batchCode, decimal quantity)
        {
            var lead = await _userRepository.GetByRoleAsync("Lead");
            if (lead is null) return;

            var title = "Thêm lô hàng mới";
            var message = $"Lô hàng mới với mã lô {batchCode} và số lượng {quantity} đã được thêm vào hệ thống.";
            var type = "Batch";

            var notification = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
