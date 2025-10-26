using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;
using NotificationDTO = Application.DTOs.NotificationDTO;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Income, IncomeDTO>().ReverseMap();
            CreateMap<Production, ProductionDTO>().ReverseMap();
            CreateMap<AssignmentDTO, Assignment>().ReverseMap();
            CreateMap<NotificationDTO, Notification>().ReverseMap();
            CreateMap<EvaluateDTO, Evaluate>().ReverseMap();
            CreateMap<ComponentDefectsDTO, ComponentDefect>().ReverseMap();
        }
    }
}
