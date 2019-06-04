using BangazonWorkforceMVC.Models;
using Microsoft.Extensions.Configuration;
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
        }
}
