using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        public new Employee? GetById(int employeeId);
        public new IEnumerable<Employee> GetAll();
    }
}
