using AutoMapper;
using ToDoApp.DTOs;
using ToDoApp.Models;
using ToDoApp.DTOs;
using ToDoApp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ToDoApp.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateTodo, Todo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            CreateMap<UpdateTodo, Todo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
        }
    }
}