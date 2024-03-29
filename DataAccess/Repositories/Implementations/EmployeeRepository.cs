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
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;

        public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override Employee? GetById(int employeeId)
        {
            return _dbContext.Employees
                        .Include(e => e.Position)
                        .Include(e => e.AssignedProjects)
                        .SingleOrDefault(e => e != null && e.Id == employeeId);
        }
        public override IEnumerable<Employee> GetAll()
        {
            return _dbContext.Employees
                        .Include(e => e.Position)
                        .Include(e => e.AssignedProjects)
                        .ToList();
        }

        public override Employee Add(Employee entity)
        {
            if (entity.PositionId != 0) entity.Position = null;
            return base.Add(entity);
        }

        public override Employee Update(Employee entity)
        {
            if (entity.PositionId != 0) entity.Position = null;
            return base.Add(entity);
        }
    }
}
