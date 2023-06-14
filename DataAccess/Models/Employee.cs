using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Surname { get; set; }
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }

        public int? PositionId { get; set; }
        public Position? Position { get; set; }
        public IEnumerable<Project> ManagedProjects { get; set; } = new List<Project>();
        public IEnumerable<Project> AssignedProjects { get; set; } = new List<Project>();
    }
}
