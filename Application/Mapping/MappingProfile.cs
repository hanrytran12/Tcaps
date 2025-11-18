using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Response;
using AutoMapper;
using Domain.Entities;

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
            CreateMap<Material, MaterialDTO>().ReverseMap();

            CreateMap<Evaluate, EvaluateDTO>()
                .ForMember(dest => dest.Created_At,
                    opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<TaskTransferRequest, TaskTransferRequestDTO>();
        }
    }
}
