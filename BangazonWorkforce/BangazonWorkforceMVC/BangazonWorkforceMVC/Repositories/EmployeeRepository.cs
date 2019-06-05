using Microsoft.Extensions.Configuration;
﻿using BangazonWorkforceMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Repositories
{
    public class EmployeeRepository
    {

        private static IConfiguration _config;

        public static void SetConfig(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public static List<Employee> GetEmployees()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.Id, e.FirstName, e.LastName, e.DepartmentId,
                        d.Id, d.Name AS 'Department'
                        FROM Employee e LEFT JOIN Department d ON e.DepartmentId = d.Id";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Department = new Department()
                            {
                                Name = reader.GetString(reader.GetOrdinal("Department")),
                            }
                        };

                        employees.Add(employee);
                    }

                    reader.Close();

                    return employees;
                }
            }
        }

        //Gets Single Employee Detail
        public static Employee GetOneEmployee(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT Employee.Id, Employee.FirstName, Employee.LastName, Computer.Make, Computer.Manufacturer, Department.[Name], TrainingProgram.[Name] AS 'Training Name', TrainingProgram.Id AS 'TPId', Computer.Id AS 'ComputerId', TrainingProgram.StartDate, TrainingProgram.EndDate FROM Employee LEFT JOIN ComputerEmployee on Employee.Id = ComputerEmployee.EmployeeId LEFT JOIN Computer ON Computer.Id = ComputerEmployee.ComputerId LEFT JOIN EmployeeTraining on Employee.Id = EmployeeTraining.EmployeeId LEFT JOIN TrainingProgram ON TrainingProgram.Id = EmployeeTraining.TrainingProgramId LEFT JOIN Department ON Employee.DepartmentId = Department.Id WHERE Employee.Id = @id AND ComputerEmployee.UnassignDate IS null
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
                            CurrentDepartment = new Department
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            }
                        };
                        //If employeeDisplayed is null, make employee employeeDisplayed
                        if (employeeDisplayed == null)
                        {
                            employeeDisplayed = employee;
                        }
                       
                        //If Computer Id is not null, build a Computer object
                        if (!reader.IsDBNull(reader.GetOrdinal("ComputerId")))
                        {
                            employee.CurrentComputer = new Computer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                Make = reader.GetString(reader.GetOrdinal("Make")),
                                Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                            };
                           
                        }

                        //If TrainingProgram Id is not null, build a TrainingProgram object
                        if (!reader.IsDBNull(reader.GetOrdinal("TPId")))
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
                    return employeeDisplayed;

                }
            }
        }
    }
}
