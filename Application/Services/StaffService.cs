using Application.DTOs.Request;
using Application.DTOs.Response;
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
