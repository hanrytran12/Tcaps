using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IStaffService
    {
        Task<ResponseDTO> GetUserProfileAsync(Guid userId);
        Task<Result> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken);
        Task<ResponseDTO> UpdateProfileAsync(Guid userId, UpdateProfileUserDTO dto, CancellationToken cancellationToken);
    }
}
