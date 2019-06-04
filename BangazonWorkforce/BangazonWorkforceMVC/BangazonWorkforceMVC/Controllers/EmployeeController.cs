using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
<<<<<<< HEAD
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BangazonAPI;
using Microsoft.Extensions.Configuration;
using BangazonAPI.Models;
=======
using BangazonWorkforceMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BangazonWorkforceMVC.Repositories;
>>>>>>> master

namespace BangazonWorkforceMVC.Controllers
{
    public class EmployeeController : Controller
    {
<<<<<<< HEAD
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
=======
        private readonly IConfiguration _config;

        public EmployeeController(IConfiguration config)
        {
            EmployeeRepository.SetConfig(config);
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

>>>>>>> master
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
<<<<<<< HEAD
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT Employee.Id, Employee.FirstName, Employee.LastName, Computer.Make, Computer.Manufacturer, Department.[Name], TrainingProgram.[Name] AS 'Training Name', TrainingProgram.Id, Computer.Id, TrainingProgram.StartDate, TrainingProgram.EndDate FROM Employee LEFT JOIN ComputerEmployee on Employee.Id = ComputerEmployee.EmployeeId LEFT JOIN Computer ON Computer.Id = ComputerEmployee.ComputerId LEFT JOIN EmployeeTraining on Employee.Id = EmployeeTraining.EmployeeId LEFT JOIN TrainingProgram ON TrainingProgram.Id = EmployeeTraining.TrainingProgramId LEFT JOIN Department ON Employee.DepartmentId = Department.Id WHERE Employee.Id = @id
        ";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Employee employeeDisplayed = null;

                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            CurrentComputer = new Computer
                            {
                                Make = reader.GetString(reader.GetOrdinal("Make")),
                                Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                            },
                            CurrentDepartment = new Department
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };
                        if (employeeDisplayed == null)
                        {
                            employeeDisplayed = employee;
                        }

                        if (employee.AssignedTraining != null)
                        {
                            TrainingProgram program = new TrainingProgram
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Training Name")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate"))
                            };
                            employeeDisplayed.AssignedTraining.Add(program);
                        }



                    };
                    reader.Close();
                    return View(employeeDisplayed);

                }
            }
        }
        
            
        
=======
            return View();
        }
>>>>>>> master

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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