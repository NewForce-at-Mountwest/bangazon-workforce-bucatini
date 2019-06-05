using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonWorkforceMVC.Models;
using BangazonWorkforceMVC.Models.ViewModels;
using BangazonWorkforceMVC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonWorkforceMVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _config;

        public EmployeeController(IConfiguration config)
        {
            EmployeeRepository.SetConfig(config);
            DepartmentRepository.SetConfig(config);
            ComputerRepository.SetConfig(config);
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: Employee
        public ActionResult Index()
        {
            {
                List<Employee> employees = EmployeeRepository.GetEmployees();
                return View(employees);
            }
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            // Create a new instance of a CreateEmployeeViewModel
            // If we want to get all the departments, we need to use the constructor that's expecting a connection string. 
            // When we create this instance, the constructor will run and get all the departments.
            CreateEmployeeViewModel createEmployeeViewModel = new CreateEmployeeViewModel(_config.GetConnectionString("DefaultConnection"));

            // Once we've created it, we can pass it to the view
            return View(createEmployeeViewModel);
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateEmployeeViewModel model)
        {
            EmployeeRepository.CreateEmployee(model);
            return RedirectToAction(nameof(Index));
        }

         // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {  
            EmployeeEditViewModel EmployeeEditViewModel = new EmployeeEditViewModel(id);   
            return View(EmployeeEditViewModel);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmployeeEditViewModel model)
        {
            try
            {
                EmployeeRepository.UpdateEmployee(id, model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View(model);
            }
        }

            // GET: Employee/Details/5
            public ActionResult Details(int id)
            {
                {
                    Employee employee = EmployeeRepository.GetEmployeeDetail(id);
                    return View(employee);
                }
            }
        
    }
}