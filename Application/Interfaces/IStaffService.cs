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
    }
}
