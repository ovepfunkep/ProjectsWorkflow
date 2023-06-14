using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? CustomerCompany { get; set; }
        public string? ExecutorCompany { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Priority { get; set; }
        public int? ProjectManagerId { get; set; }

        public Employee? ProjectManager { get; set; }
        public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();
    }
}
