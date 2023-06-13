using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic.Services.Implementations
{
    public class ProjectService : CRUDService<Project, ProjectDTO>, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository,IEmployeeRepository employeeRepository, IMapper mapper) : base(projectRepository, mapper)
        {
            _employeeRepository = employeeRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public override ProjectDTO Add(ProjectDTO projectDto)
        {
            // Map the ProjectDTO to a Project entity
            var project = _mapper.Map<Project>(projectDto);
            project.ProjectManager = null;

            project = _projectRepository.Add(project);

            _projectRepository.AddEmployees(project, 
                                            projectDto.Employees.Select(emp => _mapper.Map<Employee>(emp)));

            return _mapper.Map<ProjectDTO>(project);
        }
    }
}