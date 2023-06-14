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

        public override Project Update(Project project)
        {
            var newProject = base.Update(project);
            newProject.ProjectEmployees = project.ProjectEmployees;
            _dbContext.SaveChanges();
            return newProject;
        }

    }
}
