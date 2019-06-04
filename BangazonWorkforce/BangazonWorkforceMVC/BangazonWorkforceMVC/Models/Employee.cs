﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int DepartmentId { get; set; }
        public bool IsSupervisor { get; set; }

        public Computer CurrentComputer { get; set; } = new Computer();

        [Display(Name = "Department")]
        public Department CurrentDepartment { get; set; } = new Department();

        public List<TrainingProgram> AssignedTraining = new List<TrainingProgram>();

    }
}

