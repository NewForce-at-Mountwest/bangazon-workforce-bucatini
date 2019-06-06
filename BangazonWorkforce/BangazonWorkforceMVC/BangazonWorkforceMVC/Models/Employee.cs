using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Display(Name = "Department")]
        public Department Department { get; set; } = new Department();

        public Computer CurrentComputer { get; set; } = new Computer();

<<<<<<< HEAD
        [Display(Name = "Department")]

=======
        public List<Computer> EmployeeComputers { get; set; } = new List<Computer>();
        
>>>>>>> master
        public List<TrainingProgram> AssignedTraining = new List<TrainingProgram>();
    }
}

