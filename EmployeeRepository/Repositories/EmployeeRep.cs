using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using EmployeeApplication.Interfaces;
using EmployeeCore.Model;
using Microsoft.Extensions.Configuration;

namespace EmployeeRepository.Repositories
{
    public class EmployeeRep  : IEmployeeRepository
    {
        private readonly IConfiguration configuration;
        //public EmployeeRepository(IConfiguration configuration)
        //{
        //    this.configuration = configuration;
        //}
        public async Task<int> AddAsync(Employee entity)
        {
            entity.StartDate = DateTime.Now;
            var sql = "Insert into EMPLOYEES (FirstName,Surname,Email,StartDate,JobTitle) VALUES (@FirstName,@Surname,@Email,@StartDate,@JobTitle)";
            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Employees WHERE EmployeeId = @Id";
            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<Employee>> GetAllAsync()
        {

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EmployeeDB;Integrated Security=True;MultipleActiveResultSets=True";
                                      

            var sql = "SELECT * FROM Employees";
            try {
                using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Employee>(sql);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Employees WHERE EmployeeId = @Id";
            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Employee>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<int> UpdateAsync(Employee entity)
        {
            var sql = "UPDATE Employees SET FirstName = @FirstName, Surname = @Surname, Email = @Email, StartDate = @StartDate, JobTitle = @JobTitle, EndDate = @EndDate  WHERE EmployeeId = @Id";
            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

    }
}
