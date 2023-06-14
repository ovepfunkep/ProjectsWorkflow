using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic.Services.Implementations
{
    public class ProjectService : CRUDService<Project, ProjectDTO>, IProjectService
    {
        public ProjectService(IProjectRepository projectRepository,IEmployeeRepository employeeRepository, IMapper mapper) : base(projectRepository, mapper)
        {
        }
    }
}