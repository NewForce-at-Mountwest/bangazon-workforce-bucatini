using BangazonWorkforceMVC.Models;
using BangazonWorkforceMVC.Models.View;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
                    cmd.CommandText = @"SELECT d.Id AS 'Department Id', d.[Name] AS 'Department Name', d.Budget AS 'Budget', COUNT(e.DepartmentId) AS 'Number of Employees' FROM Department d JOIN Employee e ON d.Id = e.DepartmentId GROUP BY d.Id, d.[Name], d.Budget;";
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
    }
}
