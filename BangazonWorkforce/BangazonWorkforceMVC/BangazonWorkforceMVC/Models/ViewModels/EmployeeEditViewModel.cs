using BangazonWorkforceMVC.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Models.ViewModels
{
    public class EmployeeEditViewModel
    {
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Computers { get; set; }
        public Employee Employee { get; set; }

        public EmployeeEditViewModel() { }

        public EmployeeEditViewModel(int EmployeeId)
        {

            // When we create a new instance of this view model, we'll call the internal methods to get all the cohorts from the database
            // Then we'll map over them and convert the list of cohorts to a list of select list items

            //Query the database to get all departments

            Employee = EmployeeRepository.GetOneEmployee(EmployeeId);

            Departments = DepartmentRepository.GetAllDepartments()
                .Select(department => new SelectListItem()
                {
                    Text = department.Name,
                    Value = department.Id.ToString(),
                    

                })
                .ToList();

            // Add an option with instructions for how to use the dropdown
            Departments.Insert(0, new SelectListItem
            {
                Text = "Choose a department",
                Value = "0"
            });

            //Query the database to get all cohorts
            Computers = ComputerRepository.GetAllComputers()
                .Select(computer => new SelectListItem()
                {
                    Text = computer.Manufacturer + " " + computer.Make,
                    Value = computer.Id.ToString(),

                })
                .ToList();

            // Add an option with instructions for how to use the dropdown
            Computers.Insert(0, new SelectListItem
            {
                Text = "Choose a computer",
                Value = "0"
            });

        }        
    }
}

