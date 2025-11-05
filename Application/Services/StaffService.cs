using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Services
{
    public class StaffService : IStaffService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIncomeRepository _incomeRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IProductionRepository _productionRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly ResponseDTO _responseDTO;

        public StaffService(IUserRepository userRepository, IMapper mapper,
            IPasswordHasher passwordHasher, IUnitOfWork unitOfWork,
            IIncomeRepository incomeRepository, IAssignmentRepository assignmentRepository,
            IProductionRepository productionRepository, IBatchRepository batchRepository,
            IEvaluateRepository evaluateRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _incomeRepository = incomeRepository;
            _assignmentRepository = assignmentRepository;
            _productionRepository = productionRepository;
            _batchRepository = batchRepository;
            _evaluateRepository = evaluateRepository;
            _responseDTO = new ResponseDTO();
        }

        public async Task<ResponseDTO> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User not found";
                    return _responseDTO;
                }

                if (!_passwordHasher.Verify(currentPassword, user.PasswordHash))
                {
                    _responseDTO.StatusCode = 400;
                    _responseDTO.Message = "Current password is incorrect";
                    return _responseDTO;
                }

                var newHash = _passwordHasher.Hash(newPassword);

                user.ChangePassword(currentPassword, newHash, _passwordHasher);

                _userRepository.Update(user);
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

        public async Task<ResponseDTO> GetAssignmentsAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User not found";
                    return _responseDTO;
                }

                if (user.WorkshopId == Guid.Empty)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User has no workshop assigned.";
                    return _responseDTO;
                }

                var dto = _mapper.Map<List<AssignmentDTO>>(await _assignmentRepository.GetAssignmentsByWorkshopIdAsync(user.WorkshopId));
                _responseDTO.Data = dto;
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

        public async Task<ResponseDTO> GetEvaluateHistoryAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User not found";
                    return _responseDTO;
                }

                //lấy ds productions
                var productions = await _productionRepository.GetByUserAsync(userId);

                if (productions == null || !productions.Any())
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "Không có sản phẩm nào được sản xuất.";
                    return _responseDTO;
                }

                //lấy ds productionId
                var productionIds = productions.Select(x => x.Id).ToList();

                //lấy ds evaluate
                var evaluates = await _evaluateRepository.GetByProductionIdsAsync(productionIds);

                //lấy được ds assignIds của từng production
                var assignIds = productions.Select(x => x.AssignId).ToList();

                //lấy ds assignment dựa vào ds assignIds bên trên
                var assignments = await _assignmentRepository.GetByIdsAsync(assignIds);

                //lấy ds id của batch dựa vào assignments
                var batchIds = assignments.Select(x => x.BatchId).ToList();

                //lấy ds batch dựa vào ds batchIds ở trên
                var batches = await _batchRepository.GetBatchesByIdsAsync(batchIds);

                // Join dữ liệu lại
                var result = from eval in evaluates
                             join prod in productions on eval.ProductionId equals prod.Id
                             join assign in assignments on prod.AssignId equals assign.Id
                             join batch in batches on assign.BatchId equals batch.Id
                             select new EvaluateHistoryDTO
                             {
                                 ProductionId = eval.ProductionId,
                                 BatchCode = batch.Code,
                                 QuantityProduced = prod.Quantity,
                                 UserId = eval.UserId.Value,
                                 Status = eval.Status,
                                 Note = eval.Note,
                                 CreatedAt = eval.CreatedAt
                             };


                _responseDTO.Data = result.ToList();
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

        public async Task<ResponseDTO> GetIncomeHistoryAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User not found";
                    return _responseDTO;
                }

                var dto = _mapper.Map<List<IncomeDTO>>(await _incomeRepository.GetIncomeHistoryAsync(userId));
                _responseDTO.Data = dto;
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

        public async Task<ResponseDTO> GetProductionsAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User not found";
                    return _responseDTO;
                }

                var productions = await _productionRepository.GetByUserAsync(userId);

                if (productions == null || !productions.Any())
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "Không có sản phẩm nào được sản xuất.";
                    return _responseDTO;
                }

                var dto = _mapper.Map<List<ProductionDTO>>(productions);
                _responseDTO.Data = dto;
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

        public async Task<ResponseDTO> GetTotalIncomeAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User not found";
                    return _responseDTO;
                }

                _responseDTO.Data = await _incomeRepository.GetTotalIncomeAsync(userId);
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

        public async Task<ResponseDTO> GetUserProfileAsync(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User not found";
                    return _responseDTO;
                }
                _responseDTO.Data = _mapper.Map<UserDTO>(user);
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

        public async Task<ResponseDTO> UpdateProfileAsync(Guid userId, UpdateProfileUserDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _responseDTO.StatusCode = 404;
                    _responseDTO.Message = "User not found";
                    return _responseDTO;
                }

                user.UpdateProfile(dto.FullName, dto.Email, dto.Phone);
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
