using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonWorkforceMVC.Models.ViewModels;
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
            return View();
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            // Create a new instance of a CreateStudentViewModel
            // If we want to get all the cohorts, we need to use the constructor that's expecting a connection string. 
            // When we create this instance, the constructor will run and get all the cohorts.
            CreateEmployeeViewModel createEmployeeViewModel = new CreateEmployeeViewModel(_config.GetConnectionString("DefaultConnection"));

            // Once we've created it, we can pass it to the view
            return View(createEmployeeViewModel);
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateEmployeeViewModel model)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Employee
                ( FirstName, LastName, DepartmentId )
                VALUES
                ( @firstName, @lastName, @departmentId )";
                    cmd.Parameters.Add(new SqlParameter("@firstName", model.employee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", model.employee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@departmentId", model.employee.DepartmentId));
                    
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}