
﻿using BangazonWorkforceMVC.Models;
using BangazonWorkforceMVC.Models.View;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace BangazonWorkforceMVC.Repositories
{
    public class DepartmentRepository
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

        public static List<DepartmentEmployeeViewModel> GetDepartments()




        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    // Retrieving all of a departments info plus counting through employees with matching departmentId
                    cmd.CommandText = @"SELECT d.Id AS 'Department Id', d.[Name] AS 'Department Name', d.Budget AS 'Budget', COUNT(e.DepartmentId) AS 'Number of Employees' FROM Department d LEFT JOIN Employee e ON d.Id = e.DepartmentId GROUP BY d.Id, d.[Name], d.Budget;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<DepartmentEmployeeViewModel> DepartmentsWithEmployees = new List<DepartmentEmployeeViewModel>();
                    while (reader.Read())
                    {
                        DepartmentEmployeeViewModel currentDepartment = new DepartmentEmployeeViewModel
                        {
                            department = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Department Id")),
                                Name = reader.GetString(reader.GetOrdinal("Department Name")),
                                Budget = reader.GetInt32(reader.GetOrdinal("Budget"))
                            },
                            numberOfEmployeesAssigned = reader.GetInt32(reader.GetOrdinal("Number of Employees"))
                        };


                        DepartmentsWithEmployees.Add(currentDepartment);
                    }

                    reader.Close();

                    return DepartmentsWithEmployees;
                }
            }
        }


        public static Department CreateDepartment(Department department){
        using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Department
                ( Name, Budget )
                VALUES
                ( @name, @budget)";
                    cmd.Parameters.Add(new SqlParameter("@name", department.Name));
                    cmd.Parameters.Add(new SqlParameter("@budget", department.Budget));
                    cmd.ExecuteNonQuery();

                }
                return department;
            }
        }
        public static Department GetDepartmentDetails(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
SELECT Department.Id, Department.[Name] AS 'DeptName', Employee.Id, Employee.FirstName AS 'FirstName', Employee.LastName AS 'LastName', Employee.DepartmentId, Department.Budget FROM Department LEFT JOIN Employee ON Department.Id = Employee.DepartmentId WHERE Department.Id = @id
        ";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Department departmentDisplayed = null;

                    while (reader.Read())
                    {
                        Department department = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("DeptName")),
                            Budget = reader.GetInt32(reader.GetOrdinal("Budget"))

                        };

                        //If departmentDisplayed is null, make department departmentDisplayed
                        if (departmentDisplayed == null)
                        {
                            departmentDisplayed = department;
                        }

                        //Checks to see if departmentDisplayed has employees in the EmployeesInDepartment list. If it does, build the employee object and add it
                        if (!reader.IsDBNull(reader.GetOrdinal("FirstName")))
                        {
                            Employee employee = new Employee
                            {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            };
                            departmentDisplayed.EmployeesInDepartment.Add(employee);
                        }
                    };
                    reader.Close();
                    return departmentDisplayed;
                }
            }
        }

    public static List<Department> GetAllDepartments()
        {

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                            {
                                List<Department> allDepartments = new List<Department>();
                                cmd.CommandText = "SELECT Id, Name FROM Department";
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    List<Department> departments = new List<Department>();
                                    while (reader.Read())
                                    {
                                        departments.Add(new Department
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                            Name = reader.GetString(reader.GetOrdinal("Name")),
                                        });
                                    }

                                    reader.Close();
                                    return departments;
                                }
                            }
                        }
        }
    }
}
