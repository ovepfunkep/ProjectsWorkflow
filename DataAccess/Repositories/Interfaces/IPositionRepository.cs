using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IPositionRepository : IRepository<Position>
    {
        public Position? GetByName(string name);
    }
}
