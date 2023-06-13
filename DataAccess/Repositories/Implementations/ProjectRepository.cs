using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly AppDbContext _dbContext;

        public ProjectRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override Project? GetById(int projectId)
        {
            return _dbContext.Projects
                        .Include(p => p.ProjectManager)
                            .ThenInclude(e => e != null ? e.Position : null)
                        .Include(p => p.ProjectEmployees)
                            .ThenInclude(pe => pe.Employee)
                                .ThenInclude(e => e != null ? e.Position : null)
                        .SingleOrDefault(p => p.Id == projectId);
        }

        public override IEnumerable<Project> GetAll()
        {
            return _dbContext.Projects
                        .Include(p => p.ProjectManager)
                            .ThenInclude(e => e != null ? e.Position : null)
                        .Include(p => p.ProjectEmployees)
                            .ThenInclude(pe => pe.Employee)
                                .ThenInclude(e => e != null ? e.Position : null)
                        .ToList();
        }

        public IEnumerable<ProjectEmployee> AddEmployees(Project project, IEnumerable<Employee> employees)
        {
            List<ProjectEmployee> result = new();
            var dbProject = _dbContext.Projects.Find(project.Id) ?? _dbContext.Projects.Local.FirstOrDefault(p => p.Id == project.Id, null);
            foreach (Employee employee in employees)
            {
                var dbEmployee = _dbContext.Employees.Find(employee.Id) ?? _dbContext.Employees.Local.FirstOrDefault(e => e?.Id == employee.Id, null);
                if (dbEmployee == null) continue;
                result.Add(_dbContext.ProjectsEmployees.Add(new ProjectEmployee() { ProjectId = dbProject.Id, EmployeeId = dbEmployee.Id }).Entity);
            }
            _dbContext.SaveChanges();
            return result;
        }


        public bool RemoveEmployees(Project project, IEnumerable<Employee> employees)
        {
            foreach (Employee employee in employees)
            {
                _dbContext.Employees.Attach(employee);
                _dbContext.ProjectsEmployees.Remove(new ProjectEmployee() { Project = project, Employee = employee});
            }
            int affectedRows = _dbContext.SaveChanges();
            return affectedRows > 0;
        }
    }
}
