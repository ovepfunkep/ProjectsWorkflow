using BusinessLogic.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Interfaces
{
    public interface IPositionService : ICRUDService<Position, PositionDTO>
    {
        public PositionDTO GetByName(string name);
    }
}
