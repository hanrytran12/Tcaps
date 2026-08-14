using API.Hubs;
using Application.Common.Exceptions;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Application.Common;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMaterialSupplyRepository _materialSupplyRepository;
        private readonly IMapper _mapper;
        private readonly IProductionRepository _productionRepository;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IAppDbContext _context;
        private readonly ResponseDTO _responseDTO;

        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationServices(IUnitOfWork unitOfWork, IUserRepository userRepository, INotificationRepository notificationRepository, IMaterialRepository materialRepository, IBatchRepository batchRepository
            , IMapper mapper, IProductionRepository productionRepository, IIncomeRepository incomeRepository,
            IEvaluateRepository evaluateRepository, IWorkshopRepository workshopRepository, IComponentDefectRepository componentDefectRepository,
            IMaterialRequestRepository materialRequestRepository, IAssignmentRepository assignmentRepository,
            IMaterialSupplyRepository materialSupplyRepository, IHubContext<NotificationHub> hubContext, IAppDbContext context)
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
            _materialSupplyRepository = materialSupplyRepository;
            _mapper = mapper;
            _productionRepository = productionRepository;
            _incomeRepository = incomeRepository;
            _evaluateRepository = evaluateRepository;
            _responseDTO = new ResponseDTO();
            _hubContext = hubContext;
            _context = context;
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
            var message = $"Lead {user?.FullName} is approve request for {(int)quantityRequest} of material {material?.Name} for batch {batch?.Code}.";
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

        public async Task<Result<int>> CountNotificationAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return Result<int>.NotFound("User not found.", "user_not_found");
                }

                var count = await _notificationRepository.CountNotificationAsync(userId);
                return Result<int>.Success(count);
            }
            catch
            {
                return Result<int>.Internal("Không thể tải số lượng thông báo.", "notification_count_failed");
            }
        }

        public async Task SendStockUpdateNotificationToAdminAsync(string name, int newStockQuantity, int stockChange)
        {
            var admin = await _userRepository.GetByRoleAsync("Admin");
            var title = "Material Stock Updated";
            var message = $"Material {name} stock has been updated. New stock quantity: {newStockQuantity} (Change: {stockChange}).";
            var type = "MaterialStockUpdate";
            var notification = new Notification(Guid.NewGuid(), admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);

            await _hubContext.Clients.User(admin.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
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

            await _hubContext.Clients.User(staff.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendSubmitProductionNotification(Guid assignId, Guid userId, int quantity)
        {
            //var user = await _userRepository.GetByIdAsync(userId);
            //var qc = await _userRepository.GetQCByWorkshopIdAsync(user.WorkshopId);

            //var title = "Nộp sản phẩm";
            //var message = $"Nhân viên {user.FullName} nộp {quantity} sản phẩm để QC kiểm tra.";
            //var type = "Production";
            //var notification = new Notification(Guid.NewGuid(), qc.Id, title, message, type);
            //await _notificationRepository.AddAsync(notification);
            //await _unitOfWork.SaveChangesAsync();

            //Console.WriteLine($"🔔 Sending notification to QC User ID: {qc.Id}");
            //Console.WriteLine($"   - QC Name: {qc.FullName}");
            //Console.WriteLine($"   - Message: {message}");

            //await _hubContext.Clients.User(qc.Id.ToString()).SendAsync("ReceiveNotification", new
            //{
            //    Id = notification.Id,
            //    Title = title,
            //    Message = message,
            //    Type = type,
            //    CreatedAt = DateTime.Now
            //});
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

            await _hubContext.Clients.User(qc.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
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

            await _hubContext.Clients.User(qc.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
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

            await _hubContext.Clients.User(user.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendStockUpdateNotificationToLeadAsync(string name, int newStockQuantity, int stockChange)
        {
            var lead = await _userRepository.GetByRoleAsync("Lead");
            var title = "Material Stock Updated";
            var message = $"Material {name} stock has been updated. New stock quantity: {newStockQuantity} (Change: {stockChange}).";
            var type = "MaterialStockUpdate";
            var notification = new Notification(Guid.NewGuid(), lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);

            await _hubContext.Clients.User(lead.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task CreateStockUpdateNotificationForRoleAsync(Guid userId, string materialName, int newStock, int change)
        {
            var users = await _userRepository.GetByIdAsync(userId);
            if (users is null) return;

            var title = "Cập nhật Tồn kho Nguyên vật liệu";
            var message = $"Tồn kho của '{materialName}' đã thay đổi. Số lượng mới: {newStock} (thay đổi: {change}).";
            var type = "MATERIAL_STOCK_UPDATE";

            var notification = Notification.Create(users.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);

            await _hubContext.Clients.User(users.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendMaterialWorkshopConfirmNotificationAsync(Guid workshopId, int quantitySend, int quantityReceive, Guid? userId, string batchCode)
        {
            if (userId == null) return;
            var lead = await _userRepository.GetByIdAsync(userId.Value);
            var admin = await _userRepository.GetByRoleAsync("Admin");

            var qc = await _userRepository.GetQCByWorkshopIdAsync(workshopId);
            if (lead is null || admin is null || qc is null) return;

            var workshop = await _workshopRepository.GetByIdAsync(workshopId);

            var title = "Chấp nhận đơn hàng";
            var message = $"QC {qc.FullName} của xưởng {workshop.Name} đã tiếp nhận " +
                $"{quantitySend} sản phẩm và nhận {quantityReceive}" +
                $"từ xưởng trước thuộc lô hàng {batchCode}.";
            var type = "MaterialWorkshop";

            var notificationLead = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationLead);

            var notificationAdmin = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationAdmin);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(admin.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notificationAdmin.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });

            await _hubContext.Clients.User(lead.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notificationLead.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendCreateTaskTransferRequestNotificationAsync(Guid batchId, Guid workshopId, Guid qcTransportId, string note)
        {
            var admin = await _userRepository.GetByRoleAsync("Admin");
            var qc = await _userRepository.GetByIdAsync(qcTransportId);
            if (admin is null) return;

            var batch = await _batchRepository.GetByIdAsync(batchId);
            var workshop = await _workshopRepository.GetByIdAsync(workshopId);

            var title = "Yêu cầu chuyển giao công việc cho QC vận chuyển";
            var message = $"Lead vừa tạo yêu cầu chuyển giao cho lô {batch.Code} tại xưởng {workshop.Name} cho QC tên {qc.FullName}. Ghi chú: {note}";
            var type = "TaskTransferRequest";

            var notification = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(admin.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendApproveTaskTransferRequestNotificationAsync(Guid taskTransferRequestId, Guid qcTransportId)
        {
            var qcTransport = await _userRepository.GetByIdAsync(qcTransportId);
            if (qcTransport is null) return;

            var title = "Yêu cầu chuyển giao đã được duyệt";
            var message = $"Yêu cầu chuyển giao của Admin đã được duyệt. Vui lòng kiểm tra để tiến hành vận chuyển.";
            var type = "TaskTransferRequest";

            var notification = Notification.Create(qcTransport.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(qcTransport.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendCreateMaterialRequestNotificationAsync(Guid qcId, Guid batchId, Guid assignId)
        {
            var qc = await _userRepository.GetByIdAsync(qcId);

            var batch = await _batchRepository.GetByIdAsync(batchId);
            var workshop = await _workshopRepository.GetByIdAsync(qc.WorkshopId);

            if (batch.UserId is null) return;

            var lead = await _userRepository.GetByIdAsync(batch.UserId.Value);
            if (lead is null || qc is null) return;

            var title = "Yêu cầu cung cấp thêm vật liệu";
            var message = $"Yêu cầu cung cấp thêm vật liệu cho lô hàng {batch.Code} tại xưởng {workshop.Name}.";
            var type = "MaterialRequest";

            var notification = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(lead.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
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
            var message = $"Admin đã duyệt yêu cầu cho {qcTransport.FullName} cung cấp thêm {quantity} {material.Unit} vật liệu {material.Name} cho lô hàng {batch.Code} tại xưởng {workshop.Name}.";
            var type = "MaterialSupply";

            var notification = Notification.Create(qcTransport.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(qcTransport.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
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

            await _hubContext.Clients.User(qc.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendCompletedMaterialSupplyNotificationAsync(Guid userId, Guid materialId, string batchCode, int quantity)
        {
            var material = await _materialRepository.GetByIdAsync(materialId);
            var lead = await _userRepository.GetByIdAsync(userId);

            var title = "Hoàn tất cung cấp thêm vật liệu";
            var message = $"QC đã nhận {quantity} {material.Unit} vật liệu **{material.Name}** của đơn yêu cầu thêm NVL của lô hàng {batchCode}.";
            var type = "MaterialSupply";

            var notification = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(lead.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendQCTransportApproveMaterialSupplyNotificationAsync(Guid qcTransportId, Guid materialSupplyId)
        {
            var qcTransport = await _userRepository.GetByIdAsync(qcTransportId);

            var admin = await _userRepository.GetByRoleAsync("Admin");

            var materialSupply = await _materialSupplyRepository.GetByIdAsync(materialSupplyId);

            var materialRequest = await _materialRequestRepository.GetByIdAsync(materialSupply.RequestId);

            var batch = await _batchRepository.GetByIdAsync(materialRequest.BatchId);

            if (batch.UserId == null)
                throw new NotFoundException("Batch chưa gán Lead");

            var lead = await _userRepository.GetByIdAsync(batch.UserId.Value);

            var title = "QC vận chuyển tiếp nhận";
            var message = $"QC vận chuyển {qcTransport.FullName} đã tiếp nhận đơn yêu cầu cung cấp thêm NVL có mã đơn là {materialSupplyId}.";
            var type = "MaterialSupply";

            var notificationLead = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationLead);

            var notificationAdmin = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationAdmin);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(lead.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notificationLead.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });

            await _hubContext.Clients.User(admin.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notificationAdmin.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendQCTransportReceptionAssignmentTransferNotificationAsync(Guid qcTransportId, Guid assignTransferRequestId, Guid assignId)
        {
            var qcTransport = await _userRepository.GetByIdAsync(qcTransportId);

            var admin = await _userRepository.GetByRoleAsync("Admin");

            var assignment = await _assignmentRepository.GetByIdAsync(assignId);
            var batch = await _batchRepository.GetByIdAsync(assignment.BatchId);

            if (batch.UserId == null)
                throw new NotFoundException("Batch chưa gán Lead");

            var lead = await _userRepository.GetByIdAsync(batch.UserId.Value);

            var workshop = await _workshopRepository.GetByIdAsync(assignment.WorkshopId);

            var title = "QC vận chuyển tiếp nhận";
            var message = $"QC vận chuyển {qcTransport.FullName} đã tiếp nhận đơn chuyển giao để kiểm tra yêu cầu tại xưởng {workshop.Name}.";
            var type = "AssignmentTransferRequest";

            var notificationLead = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationLead);

            var notificationAdmin = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationAdmin);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(lead.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notificationLead.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });

            await _hubContext.Clients.User(admin.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notificationAdmin.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendQCTransportReceptionMaterialRequestNotificationAsync(Guid qcTransportId, Guid materialRequestId, Guid assignId)
        {
            var qcTransport = await _userRepository.GetByIdAsync(qcTransportId);

            var admin = await _userRepository.GetByRoleAsync("Admin");

            var assignment = await _assignmentRepository.GetByIdAsync(assignId);

            var workshop = await _workshopRepository.GetByIdAsync(assignment.WorkshopId);

            var materialRequest = await _materialRequestRepository.GetByIdAsync(materialRequestId);

            var batch = await _batchRepository.GetByIdAsync(materialRequest.BatchId);

            if (batch.UserId == null)
                throw new NotFoundException("Batch chưa gán Lead");

            var lead = await _userRepository.GetByIdAsync(batch.UserId.Value);

            var title = "QC vận chuyển tiếp nhận";
            var message = $"QC vận chuyển {qcTransport.FullName} đã tiếp nhận đơn xuất kho để giao NVL xuống xưởng {workshop.Name}.";
            var type = "AssignmentTransferRequest";

            var notificationLead = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationLead);

            var notificationAdmin = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notificationAdmin);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(lead.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notificationLead.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });

            await _hubContext.Clients.User(admin.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notificationAdmin.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendConfirmRequestFromLeadNotificationAsync(Guid materialRequestId, Guid qcId)
        {
            var qc = await _userRepository.GetByIdAsync(qcId);

            var workshop = await _workshopRepository.GetByIdAsync(qc.WorkshopId);

            var title = "Lead chấp nhận cung cấp NVL";
            var message = $"Lead đã chấp nhận yêu cầu thêm NVL cho {qc.FullName} tại xưởng {workshop.Name}.";
            var type = "MaterialSupply";

            var notification = Notification.Create(qc.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(qc.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task AddBatchNotificationAsync(Guid? userId, string batchCode, decimal quantity)
        {
            if (userId == null) return;
            var lead = await _userRepository.GetByIdAsync(userId.Value);
            if (lead is null || lead.Role != "Lead") return;

            var title = "Thêm lô hàng mới";
            var message = $"Lô hàng mới với mã lô {batchCode} và số lượng {(int)quantity} đã được thêm vào hệ thống.";
            var type = "Batch";

            var notification = Notification.Create(lead.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(lead.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task ApproveFinalTransferRequestNotificationAsync(Guid batchId, decimal quantityComplete, decimal quantityError)
        {
            var admin = await _userRepository.GetByRoleAsync("Admin");
            if (admin is null) return;

            var batch = await _batchRepository.GetByIdAsync(batchId);

            var title = "Lô hàng hoàn thành";
            var message = $"Lô hàng với mã lô {batch.Code} đã hoàn thành với số lượng đạt là {(int)quantityComplete} và số lượng lỗi là {(int)quantityError}.";
            var type = "FinalTransferRequest";

            var notification = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(admin.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task UpdateQuantityDefectNotificationAsync(decimal quantityReject, Guid qcId, string batchCode)
        {
            var qc = await _userRepository.GetByIdAsync(qcId);
            if (qc is null) return;

            var title = "Cập nhật số lượng yêu cầu làm lại";
            var message = $"Lead đã cập nhật số lượng yêu cầu làm lại vì phát hiện thêm {(int)quantityReject} lỗi của lô hàng {batchCode}.";
            var type = "AssignmentTransferRequest";

            var notification = Notification.Create(qc.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(qc.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task AssignWorkshopNotificationAsync(Guid userId, string batchCode)
        {
            var admin = await _userRepository.GetByRoleAsync("Admin");
            var lead = await _userRepository.GetByIdAsync(userId);
            if (admin is null || lead is null) return;

            var title = "Lead phân công giai đoạn";
            var message = $"Lead {lead.FullName} đã phân công giai đoạn cho lô hàng {batchCode}.";
            var type = "Assignment";

            var notification = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(admin.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendAddBatchForAdminNotificationAsync(string batchCode, decimal quantity)
        {
            var admin = await _userRepository.GetByRoleAsync("Admin");
            if (admin is null) return;

            var title = "Admin tạo lô hàng";
            var message = $"Lô hàng {batchCode} mới được tạo với số lượng yêu cầu là {(int)quantity}.";
            var type = "Batch";

            var notification = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(admin.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendUpdateQuantityProductionNotificationAsync(Guid userId, decimal quantitySend, decimal quantityReceive, DateOnly date, TimeOnly time, string batchCode)
        {
            var staff = await _userRepository.GetByIdAsync(userId);
            if (staff is null) return;

            var title = "Cập nhật sản lượng";
            var message =
                $"Sản lượng của lô {batchCode} đã được cập nhật.\n" +
                $"Số lượng bạn gửi: {(int)quantitySend}\n" +
                $"Số lượng QC đã kiểm tra: {(int)quantityReceive}\n" +
                $"Của sản phẩm có thời gian nộp: {date:dd/MM/yyyy} và giờ nộp: {time:HH:mm}.";
            var type = "Production";

            var notification = Notification.Create(staff.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(staff.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendFinalTransferRequestForGuardQCNotificationAsync(decimal quantitySend, string batchCode, string workshopName)
        {
            var qcgaccong = await _userRepository.GetByRoleAsync("GuardQC");
            if (qcgaccong is null) return;

            var title = "Kiểm tra cuối cùng";
            var message = $"QC của xưởng {workshopName} vừa nộp {(int)quantitySend} sản phẩm thuộc lô hàng {batchCode}. Xin lòng kiểm tra.";
            var type = "FinalTransferRequest";

            var notification = Notification.Create(qcgaccong.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(qcgaccong.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendProductionReportNotificationAsync(Guid assignId, Guid staffId, decimal quantity)
        {
            var staff = await _userRepository.GetByIdAsync(staffId);
            var assignment = await _assignmentRepository.GetByIdAsync(assignId);
            var qc = await _userRepository.GetQCByWorkshopIdAsync(assignment.WorkshopId);
            var batch = await _batchRepository.GetByIdAsync(assignment.BatchId);

            if (staff is null || qc is null) return;

            var title = "Nộp sản phẩm";
            var message = $"Nhân viên {staff.FullName} vừa nộp {(int)quantity} sản phẩm của lô hàng {batch.Code} cho QC đánh giá.";
            var type = "Production";

            var notification = Notification.Create(qc.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(qc.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendAddMaterialSupplyForAdminNotification(Guid materialId, decimal quantitySend, DateTime dateShip)
        {
            var admin = await _userRepository.GetByRoleAsync("Admin");
            if (admin is null) return;

            var material = await _materialRepository.GetByIdAsync(materialId);

            var title = "Duyệt yêu cầu cung cấp vật liệu cho QC vận chuyển.";
            var message = $"Vật liệu {material?.Name} được gửi với số lượng {(int)quantitySend}, ngày giao {dateShip}.";
            var type = "MaterialSupply";

            var notification = Notification.Create(admin.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(admin.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendNotificationForStaffNotificationAsync(Guid userId, Guid batchId, decimal? quantity)
        {
            var batch = await _batchRepository.GetByIdAsync(batchId);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null) return;

            if (user.WorkshopId == null)
                return;

            var staff = await _userRepository.GetUsersByWorkshopIdAsync(user.WorkshopId.Value);

            if (!staff.Any()) return;

            var title = "Xác nhận nhận vật liệu";
            var message = $"QC đã tiếp nhận NVL giao xuống với số lượng {(int)quantity} của lô hàng {batch.Code}." +
                $"Nhân viên có thể bắt đầu làm việc.";
            var type = "MaterialRequest";

            foreach (var item in staff)
            {
                var notification = Notification.Create(item.Id, title, message, type);
                await _notificationRepository.AddAsync(notification);

                await _hubContext.Clients.User(item.Id.ToString()).SendAsync("ReceiveNotification", new
                {
                    Id = notification.Id,
                    Title = title,
                    Message = message,
                    Type = type,
                    CreatedAt = DateTime.Now
                });
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SendAssignmentsPlannedNotificationAsync(List<AssignmentsInfo> assignments, string batchCode)
        {
            var workshopIds = assignments.Select(a => a.WorkshopId).ToList();
            var workshops = await _workshopRepository.GetByIdsAsync(workshopIds);
            var qcUsers = await _userRepository.GetQcsByWorkshopIdsAsync(workshopIds);

            var notificationsToSend = new List<Notification>();

            foreach (var assignmentInfo in assignments)
            {
                var workshop = workshops.FirstOrDefault(w => w.Id == assignmentInfo.WorkshopId);
                var qc = qcUsers.FirstOrDefault(u => u.WorkshopId == assignmentInfo.WorkshopId);

                if (qc is null || workshop is null) continue;

                var title = "Kế hoạch sản xuất mới";
                var message = string.Empty;

                if (assignmentInfo.ExpectedDeliveryDate.HasValue)
                {
                    string formattedDate = assignmentInfo.ExpectedDeliveryDate.Value.ToString("dd/MM/yyyy");
                    message = $"Xưởng của bạn ({workshop.Name}) vừa được phân công cho lô hàng {batchCode}. " +
                              $"Dự kiến nguyên vật liệu sẽ được giao vào ngày {formattedDate}.";
                }
                else
                {
                    message = $"Xưởng của bạn ({workshop.Name}) vừa được phân công cho lô hàng {batchCode}. " +
                              $"Vui lòng kiểm tra kế hoạch và chuẩn bị nhân lực.";
                }

                var noti = Notification.Create(qc.Id, title, message, "NEW_ASSIGNMENT");

                await _notificationRepository.AddAsync(noti);
                notificationsToSend.Add(noti);
            }

            await _unitOfWork.SaveChangesAsync();

            foreach (var noti in notificationsToSend)
            {
                await _hubContext.Clients.User(noti.UserId.ToString()).SendAsync("ReceiveNotification", new
                {
                    Id = noti.Id,
                    Title = noti.Title,
                    Message = noti.Message,
                    Type = noti.Type,
                    CreatedAt = DateTime.Now
                });
            }
        }

        public async Task SendTransferRequestNotificationAsync(Guid userId, Guid assignmentId)
        {
            var userQc = await _userRepository.GetByIdAsync(userId);
            var workshop = await _workshopRepository.GetByIdAsync(userQc.WorkshopId);
            var user = await (from a in _context.Assignments
                              join b in _context.Batches on a.BatchId equals b.Id
                              join u in _context.Users on b.UserId equals u.Id
                              where a.Id == assignmentId
                              select u)
                              .FirstOrDefaultAsync();

            var title = "Yêu cầu duyệt chuyển giao mới";
            var message = $"QC {userQc.FullName} ở xưởng {workshop.Name} vừa gửi một yêu cầu duyệt công đoạn. Vui lòng kiểm tra.";
            var type = "TRANSFER_REQUEST";

            var noti = Notification.Create(user.Id, title, message, type);
            await _notificationRepository.AddAsync(noti);

            await _hubContext.Clients.User(user.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = noti.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task NotifyAdminDashboardRefreshAsync(Guid requestId)
        {
            var user = await _userRepository.GetByRoleAsync("Admin");
            await _hubContext.Clients.User(user.Id.ToString()).SendAsync("RefreshDashboard", new
            {
                RequestId = requestId
            });
        }

        public async Task SendIncomingMaterialNotificationAsync(decimal quantity, Guid assignmentId, string materialName, string unitMaterial)
        {
            var query = from a in _context.Assignments
                        where a.Id == assignmentId
                        join u in _context.Users on a.WorkshopId equals u.WorkshopId
                        where u.Role == "QC"
                        join b in _context.Batches on a.BatchId equals b.Id
                        select new { a, b.Code, u.Id };

            var queryInfo = await query.FirstOrDefaultAsync();

            var assignment = queryInfo.a;
            DateOnly? expectedDeliveryDate;
            if (assignment.Status != "Reworking")
            {
                expectedDeliveryDate = assignment.ExpectedDeliveryDate;
            }
            else
            {
                var reworkRequest = await _context.ReworkRequests.Where(rr => rr.AssignmentId == assignment.Id).FirstOrDefaultAsync();
                expectedDeliveryDate = reworkRequest?.DeliveryDate;
            }

            var type = "MATERIAL_DELIVERY_INCOMING";
            var title = "Thông báo nhận nguyên vật liệu";
            var message = $"Nguyên vật liệu {materialName} với số lượng {quantity} {unitMaterial} sẽ được giao tới xưởng bạn vào ngày {expectedDeliveryDate.Value.ToString("dd/MM/yyyy")} để làm sản phẩm cho lô hàng {queryInfo.Code}.";

            var noti = Notification.Create(queryInfo.Id, title, message, type);
            await _notificationRepository.AddAsync(noti);

            await _hubContext.Clients.User(queryInfo.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = noti.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task BroadcastContributionUpdateAsync(Guid assignId, Guid staffId, decimal quantity)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(assignId);
            var staff = await _userRepository.GetByIdAsync(staffId);

            string groupName = $"Workshop_{assignment.WorkshopId}";

            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveContributionUpdate", new
            {
                AssignmentId = assignId,
                BatchId = assignment.BatchId,
                Contributor = new
                {
                    Id = staff.Id,
                    Name = staff.FullName,
                    QuantityJustDone = quantity,
                }
            });
        }

        public async Task QCOnReworkRequestApproveNotificationAsync(Guid qcId, DateOnly deliveryDate, DateOnly endDate)
        {
            var qc = await _userRepository.GetByIdAsync(qcId);
            if (qc.WorkshopId == null) return;

            var usersToNotify = await _context.Users
                .AsNoTracking()
                .Where(u => u.WorkshopId == qc.WorkshopId || u.Id == qc.Id)
                .Distinct()
                .ToListAsync();

            var type = "REWORK_REQUEST_APPROVED";
            var title = "Yêu cầu làm lại sản phẩm lỗi";
            var message = $"Xưởng bạn sẽ làm lại sản phẩm lỗi. Nhận nguyên vật liệu vào ngày {deliveryDate.ToString("dd/MM/yyyy")} và hoàn thành trước ngày {endDate.ToString("dd/MM/yyyy")}";

            var notificationsToSend = new List<Notification>();

            foreach (var user in usersToNotify)
            {
                if (user.Role == "QC" || user.Role == "Staff")
                {
                    var noti = Notification.Create(user.Id, title, message, type);
                    await _notificationRepository.AddAsync(noti);
                    notificationsToSend.Add(noti);
                }
            }

            foreach (var notification in notificationsToSend)
            {
                await _hubContext.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", new
                {
                    Id = notification.Id,
                    Title = notification.Title,
                    Message = notification.Message,
                    Type = notification.Type,
                    CreatedAt = DateTime.Now
                });
            }
        }

        public async Task LeadOnReworkRequestAddedNotificationAsync(Guid assignmentId, Guid qcId, decimal defectiveQuantity, string noteQC)
        {
            var qcUser = await _context.Users.Where(u => u.Id == qcId).FirstOrDefaultAsync();

            var query = from a in _context.Assignments
                        where a.Id == assignmentId
                        join b in _context.Batches on a.BatchId equals b.Id
                        join w in _context.Workshop on a.WorkshopId equals w.Id
                        select new { batchCode = b.Code, workshopName = w.Name, LeadId = b.UserId };

            var assignmentInfo = await query.AsNoTracking().FirstOrDefaultAsync();

            if (assignmentInfo.LeadId == null) return;
            var leadUser = await _userRepository.GetByIdAsync(assignmentInfo.LeadId.Value);
            var batchCode = assignmentInfo.batchCode;
            var workshopName = assignmentInfo.workshopName;

            var type = "REWORK_REQUEST_CREATED";
            var title = "Yêu cầu thông báo có sản phẩm lỗi khi chuyển giao";
            var message = $"Lô hàng thuộc lô {batchCode} ở xưởng {workshopName} của QC {qcUser.FullName} hiện tại đang có {defectiveQuantity} sản phẩm lỗi, ghi chú từ QC: {noteQC}";

            var noti = Notification.Create(leadUser.Id, title, message, type);
            await _notificationRepository.AddAsync(noti);

            await _hubContext.Clients.User(leadUser.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = noti.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task CreateMaterialWorkshopNotificationAsync(Guid workshopPreviousId, Guid workshopAfterId, decimal quantitySend, string batchCode)
        {
            var nextQc = await _userRepository.GetQCByWorkshopIdAsync(workshopAfterId);
            if (nextQc is null) return;

            var workshop = await _workshopRepository.GetByIdAsync(workshopPreviousId);

            var type = "MaterialWorkshop";
            var title = "Xưởng trước gửi sản phẩm";
            var message = $"QC xưởng {workshop.Name} đã gửi sản phẩm đến xưởng của bạn với số lượng: {quantitySend}" +
                $"thuộc lô hàng {batchCode}";

            var noti = Notification.Create(nextQc.Id, title, message, type);
            await _notificationRepository.AddAsync(noti);

            await _hubContext.Clients.User(nextQc.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = noti.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task RejectComponentDefectNotificationAsync(Guid componentId, Guid evaluateId, int quantity, int quantityReject)
        {
            var evaluate = await _evaluateRepository.GetByIdAsync(evaluateId);

            var qc = await _userRepository.GetByIdAsync(evaluate.UserId.Value);

            var production = await _productionRepository.GetByIdAsync(evaluate.ProductionId);

            var user = await _userRepository.GetByIdAsync(production.UserId);

            var title = "Nộp sản phẩm đã sửa lỗi bị QC từ chối.";
            var message = $"QC chấp nhận {quantity - quantityReject} sản phẩm đạt và có {quantityReject} chưa đạt. Vui lòng kiểm tra và sửa chữa lại.";
            var type = "Production";
            var notification = new Notification(Guid.NewGuid(), user.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(qc.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendComponentRejectResolvedNotificationAsync(Guid componentId, Guid evaluateId, int quantity, int quantityReject)
        {
            var evaluate = await _evaluateRepository.GetByIdAsync(evaluateId);

            var qc = await _userRepository.GetByIdAsync(evaluate.UserId.Value);

            var production = await _productionRepository.GetByIdAsync(evaluate.ProductionId);

            var user = await _userRepository.GetByIdAsync(production.UserId);

            var title = "Nhân viên đã sửa xong lỗi bị từ chối.";
            var message = $"Nhân viên {user.FullName} đã sửa xong {quantityReject} sản phẩm bị từ chối trước đó. Vui lòng vào xác nhận (confirm) lại.";
            var type = "Production";
            var notification = new Notification(Guid.NewGuid(), qc.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(qc.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }

        public async Task SendMaterialShortageNotificationAsync(
            Guid assignId,
            Guid staffId,
            Guid materialId,
            string materialName,
            string materialUnit,
            decimal quantityRemaining,
            Guid workshopId)
        {
            var staff = await _userRepository.GetByIdAsync(staffId);
            if (staff is null) return;

            var qc = await _userRepository.GetQCByWorkshopIdAsync(workshopId);
            if (qc is null) return;

            var assignment = await _assignmentRepository.GetByIdAsync(assignId);
            var batch = await _batchRepository.GetByIdAsync(assignment.BatchId);

            var title = "Cảnh báo: Hết nguyên vật liệu";
            var message = $"Nhân viên {staff.FullName} báo cáo đã hết vật liệu \"{materialName}\" " +
                          $"(yêu cầu thêm: {quantityRemaining} {materialUnit}) cho lô hàng {batch.Code}. " +
                          $"Vui lòng cung cấp thêm nguyên vật liệu.";
            var type = "MaterialShortage";

            var notification = Notification.Create(qc.Id, title, message, type);
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _hubContext.Clients.User(qc.Id.ToString()).SendAsync("ReceiveNotification", new
            {
                Id = notification.Id,
                Title = title,
                Message = message,
                Type = type,
                CreatedAt = DateTime.Now
            });
        }
    }
}

