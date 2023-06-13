using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories.Implementations
{
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        private readonly AppDbContext _dbContext;

        public PositionRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Position? GetByName(string name) => _dbContext
                                                    .Set<Position>()
                                                    .FirstOrDefault(p => p.Name == name);
    }
}
