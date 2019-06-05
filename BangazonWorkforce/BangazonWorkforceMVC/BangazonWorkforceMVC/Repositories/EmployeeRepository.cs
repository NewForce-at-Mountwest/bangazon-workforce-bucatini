using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BangazonWorkforceMVC.Models;
using BangazonWorkforceMVC.Models.ViewModels;

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

        public static void CreateEmployee(CreateEmployeeViewModel model)
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
                }
            }
        }

        public static List<Employee> GetEmployees()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            e.Id, e.FirstName, e.LastName, e.DepartmentId,
                            c.Id AS 'ComputerId'
                        FROM Employee e
                        JOIN ComputerEmployee ce ON ce.EmployeeId = e.Id
                        JOIN Computer c ON c.Id = ce.ComputerId";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                            CurrentComputer = new Computer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ComputerId"))
                            }
                        };

                        employees.Add(employee);
                    }

                    reader.Close();

                    return employees;
                }
            }

        }

        public static Employee GetOneEmployee(int id)
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
                            c.Manufacturer,
                            ce.AssignDate,
                            ce.UnassignDate
                            FROM Employee e 
                            LEFT JOIN ComputerEmployee ce ON e.Id = ce.EmployeeId
                            LEFT JOIN Computer c ON c.Id = ce.ComputerId
                            WHERE e.Id = @id AND ce.AssignDate IS NOT NULL AND ce.UnassignDate IS NULL";
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

                        if (!reader.IsDBNull(reader.GetOrdinal("ComputerId")))
                        {
                            employee.CurrentComputer.Id = reader.GetInt32(reader.GetOrdinal("ComputerId"));
                        };
                    }
                    reader.Close();

                    return employee;
                }
            }

        }

        public static void UpdateEmployee (int id, EmployeeEditViewModel model)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Update the employee's basic info
                    string command = @"UPDATE Employee
                                            SET FirstName = @firstName, 
                                            LastName = @lastName, 
                                            IsSupervisor = @isSupervisor, 
                                            DepartmentId = @departmentId
                                            WHERE Id = @id";



                    // Get the employee's information before edit
                    Employee uneditedEmployee = EmployeeRepository.GetOneEmployee(id);

                    if (uneditedEmployee.CurrentComputer.Id != model.Employee.CurrentComputer.Id)
                    {
                        command += $" INSERT INTO ComputerEmployee (EmployeeId, ComputerId, AssignDate, UnassignDate) VALUES (@id, @computerId, @dateTime, NULL)";
                        command += $" UPDATE ComputerEmployee SET UnassignDate = @dateTime WHERE EmployeeId = @id AND ComputerId != @computerId";

                    };

                    DateTime dateTimeVariable = DateTime.Now;
                    cmd.Parameters.AddWithValue("@dateTime", dateTimeVariable);
                    cmd.Parameters.Add(new SqlParameter("@firstName", model.Employee.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", model.Employee.LastName));
                    cmd.Parameters.Add(new SqlParameter("@isSupervisor", model.Employee.IsSupervisor));
                    cmd.Parameters.Add(new SqlParameter("@departmentId", model.Employee.DepartmentId));
                    cmd.Parameters.Add(new SqlParameter("@computerId", model.Employee.CurrentComputer.Id));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.CommandText = command;
                    int rowsAffected = cmd.ExecuteNonQuery();

                }
            }
        }
    

        public static Employee GetOneEmployeeWithComputers(int id)
        {
            Employee employee = GetOneEmployee(id);
            employee.EmployeeComputers = ComputerRepository.GetAllEmployeeComputers(id);
            return employee;
        }

        //        public static void CreateStudent(CreateStudentViewModel model)
        //        {
        //            using (SqlConnection conn = Connection)
        //            {
        //                conn.Open();
        //                using (SqlCommand cmd = conn.CreateCommand())
        //                {
        //                    cmd.CommandText = @"INSERT INTO Student
        //                ( FirstName, LastName, SlackHandle, CohortId )
        //                VALUES
        //                ( @firstName, @lastName, @slackHandle, @cohortId )";
        //                    cmd.Parameters.Add(new SqlParameter("@firstName", model.student.FirstName));
        //                    cmd.Parameters.Add(new SqlParameter("@lastName", model.student.LastName));
        //                    cmd.Parameters.Add(new SqlParameter("@slackHandle", model.student.SlackHandle));
        //                    cmd.Parameters.Add(new SqlParameter("@cohortId", model.student.CohortId));
        //                    cmd.ExecuteNonQuery();


        //                }
        //            }

        //        }

        //        public static void UpdateStudent(int id, EditStudentViewModel viewModel)
        //        {
        //            using (SqlConnection conn = Connection)
        //            {
        //                conn.Open();
        //                using (SqlCommand cmd = conn.CreateCommand())
        //                {
        //                    // Update the student's basic info
        //                    string command = @"UPDATE Student
        //                                            SET firstName=@firstName, 
        //                                            lastName=@lastName, 
        //                                            slackHandle=@slackHandle, 
        //                                            cohortId=@cohortId
        //                                            WHERE Id = @id";



        //                    // Get all the exercises that WERE assigned to the student before we edited
        //                    List<StudentExercise> previouslyAssignedExercises = ExerciseRepository.GetAssignedExercisesByStudent(id);

        //                    // Loop through the exercises that we just assigned 
        //                    viewModel.SelectedExercises.ForEach(exerciseId =>
        //                    {
        //                        // Was the exercise already assigned? 
        //                        // If so, do nothing-- we want to leave it alone so we can hold onto its completion status
        //                        // If not, create a new StudentExercise entry in the DB
        //                        if (!previouslyAssignedExercises.Any(studentExercise => studentExercise.exerciseId == exerciseId))
        //                        {
        //                            command += $" INSERT INTO StudentExercise (studentId, exerciseId, isComplete) VALUES (@id, {exerciseId}, 0)";

        //                        }
        //                    });

        //                    // Loop through previously assigned exercises and check if they're still assigned. If not, delete them.
        //                    previouslyAssignedExercises.ForEach(studentExercise =>
        //                    {
        //                        if (!viewModel.SelectedExercises.Any(exerciseId => exerciseId == studentExercise.exerciseId))
        //                        {
        //                            command += $" DELETE FROM StudentExercise WHERE studentId=@id AND exerciseId={studentExercise.exerciseId}";
        //                        }

        //                    });

        //                    cmd.CommandText = command;
        //                    cmd.Parameters.Add(new SqlParameter("@firstName", viewModel.student.FirstName));
        //                    cmd.Parameters.Add(new SqlParameter("@lastName", viewModel.student.LastName));
        //                    cmd.Parameters.Add(new SqlParameter("@slackHandle", viewModel.student.SlackHandle));
        //                    cmd.Parameters.Add(new SqlParameter("@cohortId", viewModel.student.CohortId));
        //                    cmd.Parameters.Add(new SqlParameter("@id", id));
        //                    int rowsAffected = cmd.ExecuteNonQuery();

        //                }

        //            }


        //        }

        //        public static void DeleteStudent(int id)
        //        {
        //            using (SqlConnection conn = Connection)
        //            {
        //                conn.Open();
        //                using (SqlCommand cmd = conn.CreateCommand())
        //                {
        //                    cmd.CommandText = @"DELETE FROM StudentExercise WHERE studentId = @id";
        //                    cmd.Parameters.Add(new SqlParameter("@id", id));

        //                    int rowsAffected = cmd.ExecuteNonQuery();

        //                }
        //                using (SqlCommand cmd = conn.CreateCommand())
        //                {
        //                    cmd.CommandText = @"DELETE FROM Student WHERE Id = @id";
        //                    cmd.Parameters.Add(new SqlParameter("@id", id));

        //                    int rowsAffected = cmd.ExecuteNonQuery();

        //                }

        //            }

        //        }

        //        public static void MarkExerciseAsComplete(StudentExercise studentExercise)
        //        {
        //            using (SqlConnection conn = Connection)
        //            {
        //                conn.Open();
        //                using (SqlCommand cmd = conn.CreateCommand())
        //                {
        //                    cmd.CommandText = @"UPDATE StudentExercise
        //                                            SET isComplete=@completed
        //                                            WHERE studentId = @studentId
        //                                            AND exerciseId=@exerciseId";
        //                    cmd.Parameters.Add(new SqlParameter("@studentId", studentExercise.studentId));
        //                    cmd.Parameters.Add(new SqlParameter("@exerciseId", studentExercise.exerciseId));
        //                    cmd.Parameters.Add(new SqlParameter("@completed", studentExercise.isComplete ? 1 : 0));

        //                    cmd.ExecuteNonQuery();

        //                }
        //            }
        //        }
    }
}
