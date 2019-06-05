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
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Employee
                ( FirstName, LastName, IsSupervisor, DepartmentId )
                VALUES
                ( @firstName, @lastName, @isSupervisor, @departmentId )";
                    cmd.Parameters.Add(new SqlParameter("@firstName", model.Employee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", model.Employee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@isSupervisor", model.Employee.IsSupervisor));
                    cmd.Parameters.Add(new SqlParameter("@departmentId", model.Employee.DepartmentId));
                    
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = @"
                        SELECT
                            e.Id, 
                            e.FirstName,
                            e.LastName, 
                            e.IsSupervisor, 
                            e.DepartmentId, 
                            c.Id AS 'ComputerId',
                            c.Make,
                            c.Manufacturer
                            FROM Employee e 
                            LEFT JOIN ComputerEmployee ce ON e.Id = ce.EmployeeId
                            LEFT JOIN Computer c ON c.Id = ce.ComputerId
                            WHERE e.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Employee employee = null;

                    if (reader.Read())
                    {
                        employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            IsSupervisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))
                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("ComputerId"))) {
                            employee.CurrentComputer.Id = reader.GetInt32(reader.GetOrdinal("ComputerId"));
                            //                    //Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                            //                    //Make = reader.GetString(reader.GetOrdinal("Make"))
                            //                  }
                        };
                    }
                    reader.Close();

                    EmployeeEditViewModel EmployeeEditViewModel = new EmployeeEditViewModel(id);
                       
                    return View(EmployeeEditViewModel);
                }
            }
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmployeeEditViewModel model)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Employee
                                            SET FirstName = @firstName, 
                                            LastName = @lastName, 
                                            IsSupervisor = @isSupervisor, 
                                            DepartmentId = @departmentId
                                            WHERE Id = @id";
                        //cmd.Parameters.Add(new SqlParameter("@id", id));

                        // Get all the computers that WERE assigned to the employee before we edited
                        List<Computer> previouslyAssignedComputers = ComputerRepository.GetAllEmployeeComputers(id);

                        DateTime dateTimeVariable = DateTime.Now;
                        
                        // Loop through all computers the employee has used
                        //previouslyAssignedComputers.ForEach(computerId =>
                        {
                            // Was the computer already assigned? 
                            // If so, do nothing-- we want to leave it alone so we can hold onto its assigned date
                            // If not, create a new EmployeeComputer entry in the DB
                            if (!previouslyAssignedComputers.Any(computer => computer.Id == model.Employee.CurrentComputer.Id))
                            {
                                cmd.CommandText += $" INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate) VALUES (@id, @computerId, @dateTime)";
                                cmd.CommandText += $" UPDATE ComputerEmployee SET UnassignDate = @dateTime WHERE EmployeeId = @id AND ComputerId != @computerId";

                            }
                        };

                        cmd.Parameters.AddWithValue("@dateTime", dateTimeVariable);
                        cmd.Parameters.Add(new SqlParameter("@firstName", model.Employee.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", model.Employee.LastName));
                        cmd.Parameters.Add(new SqlParameter("@isSupervisor", model.Employee.IsSupervisor));
                        cmd.Parameters.Add(new SqlParameter("@departmentId", model.Employee.DepartmentId));
                        cmd.Parameters.Add(new SqlParameter("@computerId", model.Employee.CurrentComputer.Id));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();

                    }
                }
                return RedirectToAction(nameof(Edit));
            }
            catch(Exception e)
            {
                return View(model);
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