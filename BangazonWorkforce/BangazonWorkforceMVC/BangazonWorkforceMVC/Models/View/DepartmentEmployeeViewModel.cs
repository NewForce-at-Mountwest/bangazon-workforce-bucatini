﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Models.View
{
    public class DepartmentEmployeeViewModel
    {
        [Display(Name = "Number of Employees Assigned:")]
        //Need to finish up view model for looping over employees to get number of people attending.
        public int numberOfEmployeesAssigned { get; set; }

        

    }
}
