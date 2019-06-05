using BangazonWorkforceMVC.Models;
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
        public static List<Department> GetEmployees()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT d.Id AS 'Department Id, d.[Name] AS 'Department Name', d.Budget AS 'Budget', e.Id AS 'Employee Id' FROM Department d JOIN Employee e ON e.DepartmentId = d.Id;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Department> DepartmentsWithEmployees = new List<Department>();
                    while (reader.Read())
                    {
                        Department currentDepartment = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Department Id")),
                            Name = reader.GetString(reader.GetOrdinal("Department Name")),
                            Budget = reader.GetInt32(reader.GetOrdinal("Budget"))

                        };

                        Employee currentEmployee = new Employee
                        {
                             Id = reader.GetInt32(reader.GetOrdinal("Employee Id"))
                        };
                        
                        if(DepartmentsWithEmployees.Any(d => d.Id == currentDepartment.Id))
                        {

                        }
                        else
                        {

                        }
                        
                    }

                    reader.Close();

                    return employees;
                }
            }
        }
    }
}
