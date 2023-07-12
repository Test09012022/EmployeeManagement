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
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Employees WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<Employee>> GetAllAsync()
        {
            var sql = "SELECT * FROM Employees";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Employee>(sql);
                return result.ToList();
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Employees WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<Employee>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<int> UpdateAsync(Employee entity)
        {
            var sql = "UPDATE Employees SET FirstName = @FirstName, Surname = @Surname, Email = @Email, StartDate = @StartDate, JobTitle = @JobTitle, EndDate = @EndDate  WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

    }
}
