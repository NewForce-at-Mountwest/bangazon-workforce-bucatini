//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BangazonWorkforceMVC.Models.ViewModels
//{
//    public class IndexEmployeeDepartmentViewModel
//    {
//        public List<Employee> employees { get; set; }
//        public Department department { get; set; }

//        private string _connectionString;

//        private SqlConnection Connection
//        {
//            get
//            {
//                return new SqlConnection(_connectionString);
//            }
//        }

//        public StudentInstructorViewModel(string connectionString)
//        {
//            _connectionString = connectionString;
//            GetAllStudents();
//            GetAllInstructors();
//        }

//        private void GetAllStudents()
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = @"
//                        SELECT
//                            Id,
//                            FirstName,
//                            LastName,
//                            DepartmentId,
//                            IsSupervisor
//                        FROM Employee";
//                    SqlDataReader reader = cmd.ExecuteReader();

//                    employees = new List<Employee>();
//                    while (reader.Read())
//                    {
//                        employees.Add(new Employee
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))

//                        });
//                    }

//                    reader.Close();
//                }
//            }
//        }

//        private void GetAllInstructors()
//        {
//            using (SqlConnection conn = Connection)
//            {
//                conn.Open();
//                using (SqlCommand cmd = conn.CreateCommand())
//                {
//                    cmd.CommandText = @"
//                            SELECT
//                            Id,
//                            FirstName,
//                            LastName,
//                            SlackHandle
//                        FROM Instructor";
//                    SqlDataReader reader = cmd.ExecuteReader();

//                    Instructors = new List<Instructor>();
//                    while (reader.Read())
//                    {
//                        Instructors.Add(new Instructor
//                        {
//                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
//                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
//                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
//                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
//                        });
//                    }

//                    reader.Close();
//                }
//            }
//        }
//    }
//}
