using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int DepartmentId { get; set; }
        public bool IsSupervisor { get; set; }

        public Computer CurrentComputer { get; set; } = new Computer();

        public List<Computer> EmployeeComputers { get; set; } = new List<Computer>();
    }
}

