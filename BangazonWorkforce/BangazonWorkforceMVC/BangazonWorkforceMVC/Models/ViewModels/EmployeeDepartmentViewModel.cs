/*using BangazonAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Models.ViewModels
{
    public class EmployeeComputerViewModel
    {
        public Employee employee { get; set; }
        public List<TrainingProgram> Programs { get; set; }

        private string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public EmployeeComputerViewModel(string connectionString)
        {
            _connectionString = connectionString;

            Programs = GetAllPrograms()
              .Add(program => new TrainingProgram()
              {
                  Name = program.name,
                  StartDate = program.StartDate.ToString(),
                  EndDate = program.EndDate.ToString

              })
              .ToList();
        }

        private void GetAllPrograms()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT
                            Name
                        FROM Department LEFT JOIN Employee ON Department.Id = Employee.DepartmentId WHERE Department.Id = @id";
                    SqlDataReader reader = cmd.ExecuteReader();
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    Department department = null;

                    if (reader.Read())
                    {
                        department = new Department
                        {
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }

                    reader.Close();
                }
            }
        }
    }
}
*/