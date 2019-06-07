using BangazonWorkforceMVC.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforceMVC.Repositories
{
    public class ComputerRepository
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

        public static List<Computer> GetAllComputers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Manufacturer, Make FROM Computer";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Computer> computers = new List<Computer>();
                    while (reader.Read())
                    {
                        computers.Add(new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                            Make = reader.GetString(reader.GetOrdinal("Make"))
                        });
                    }

                    reader.Close();

                    return computers;
                }
            }
        }

        public static List<Computer> GetAllEmployeeComputers(int id)
    {
        using (SqlConnection conn = Connection)
        {
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                        SELECT
                            c.Id AS 'ComputerId', c.Manufacturer, c.Make
                            FROM Computer c
                            JOIN ComputerEmployee ce ON ce.ComputerId = c.Id
                            JOIN Employee e ON e.Id = ce.EmployeeId
                            WHERE e.Id = @id";
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader reader = cmd.ExecuteReader();

                    List<Computer> EmployeeComputers = new List<Computer>();

                while (reader.Read())
                {
                    Computer Computer = new Computer
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                        Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                        Make = reader.GetString(reader.GetOrdinal("Make"))
                    };

                    EmployeeComputers.Add(Computer);
                }

                reader.Close();
                return EmployeeComputers;
            }
        }

    }

    
}
}
