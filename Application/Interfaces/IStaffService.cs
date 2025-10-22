using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IStaffService
    {
        Task<ResponseDTO> GetUserProfileAsync(Guid userId);
        Task<ResponseDTO> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken);
        Task<ResponseDTO> UpdateProfileAsync(Guid userId, UpdateProfileUserDTO dto, CancellationToken cancellationToken);
        Task<ResponseDTO> GetTotalIncomeAsync(Guid userId);
        Task<ResponseDTO> GetIncomeHistoryAsync(Guid userId);
        Task<ResponseDTO> GetAssignmentsAsync(Guid userId);
        Task<ResponseDTO> GetProductionsAsync(Guid userId);
        Task<ResponseDTO> GetEvaluateHistoryAsync(Guid userId);
    }
}
