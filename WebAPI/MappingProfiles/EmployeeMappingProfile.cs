using AutoMapper;
using DataAccess.Models;
using BusinessLogic.DTOs;

namespace WebAPI.MappingProfiles
{
    /// <summary>
    /// Профиль преобразования объектов классов Employee и DTO
    /// </summary>
    public class EmployeeMappingProfile : Profile
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public EmployeeMappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeDTO, Employee>()
                .ForMember(employee => employee.PositionId, 
                           opt => opt.MapFrom(dto => dto.Position.Id));
        }
    }
}
