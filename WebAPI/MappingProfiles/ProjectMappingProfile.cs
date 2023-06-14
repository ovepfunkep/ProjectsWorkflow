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
            CreateMap<Project, ProjectDTO>();
            CreateMap<ProjectDTO, Project>()
                .ForMember(p => p.ProjectManagerId, opt => opt.MapFrom(dto => Convert.ToInt32(dto.ProjectManager.Id)));
        }
    }
}
