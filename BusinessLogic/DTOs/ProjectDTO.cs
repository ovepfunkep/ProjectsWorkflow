using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ProjectDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; } = null!;
        public string? CustomerCompany { get; set; } = null!;
        public string? ExecutorCompany { get; set; } = null!;
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? Priority { get; set; }
        public EmployeeDTO? ProjectManager { get; set; }
        public IEnumerable<EmployeeDTO> Employees { get; set; } = new List<EmployeeDTO>();
    }
}
