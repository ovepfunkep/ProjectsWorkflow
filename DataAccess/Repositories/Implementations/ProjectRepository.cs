﻿using DataAccess.Models;
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
                        .Include(p => p.Employees)
                                .ThenInclude(e => e != null ? e.Position : null)
                        .SingleOrDefault(p => p.Id == projectId);
        }

        public override IEnumerable<Project> GetAll()
        {
            return _dbContext.Projects
                        .Include(p => p.ProjectManager)
                            .ThenInclude(e => e != null ? e.Position : null)
                        .Include(p => p.Employees)
                                .ThenInclude(e => e != null ? e.Position : null)
                        .ToList();
        }

        public override Project Add(Project entity)
        {
            if (entity.ProjectManagerId != 0) entity.ProjectManager = null;
            if (entity.ProjectManager != null && entity.ProjectManager.PositionId != 0)
                entity.ProjectManager.Position = null;
            foreach (var employee in entity.Employees)
                employee.Position = null;
            return base.Add(entity);
        }
        public override Project Update(Project entity)
        {
            if (entity.ProjectManagerId != 0) entity.ProjectManager = null;
            if (entity.ProjectManager != null && entity.ProjectManager.PositionId != 0)
                entity.ProjectManager.Position = null;
            foreach (var employee in entity.Employees)
                employee.Position = null;
            return base.Add(entity);
        }
    }
}
