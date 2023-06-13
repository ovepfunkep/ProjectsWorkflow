using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.Models;

namespace WebAPI.MappingProfiles
{
    /// <summary>
    /// Профиль преобразования объектов классов Project и DTO
    /// </summary>
    public class ProjectMappingProfile : Profile
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectDTO>()
                .ForMember(dto => dto.Employees, opt => opt.MapFrom(p => p.ProjectEmployees.Select(pe => pe.Employee)));
            CreateMap<ProjectDTO, Project>()
                .ForMember(p => p.ProjectEmployees, 
                           opt => opt.MapFrom(dto => dto.Employees.Select(e => new ProjectEmployee() { EmployeeId = Convert.ToInt32(e.Id), 
                                                                                                       ProjectId = Convert.ToInt32(dto.Id)}))); 
        }
    }
}
