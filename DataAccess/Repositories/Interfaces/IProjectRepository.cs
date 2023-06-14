using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        public new Project? GetById(int projectId);

        public new IEnumerable<Project> GetAll();
    }
}
