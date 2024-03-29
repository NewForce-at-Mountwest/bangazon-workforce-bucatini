﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Models
{
    public class Department
    {
        public int Id { get; set; }

        public int Budget { get; set; }

        public string Name { get; set; }


        [Display(Name = "Employees in Department")]
        public List<Employee> EmployeesInDepartment { get; set; } = new List<Employee>();
    }
}
