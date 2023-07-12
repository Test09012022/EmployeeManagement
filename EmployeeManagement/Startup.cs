using EmployeeApplication.Interfaces;
using EmployeeRepository.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
[assembly: FunctionsStartup(typeof(EmployeeManagement.Startup))]
namespace EmployeeManagement
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EmployeeDB;Integrated Security=True;MultipleActiveResultSets=True";

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
           
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRep>();
            // builder.Services.AddInfrastructure();


        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure();
        }

        }
}
