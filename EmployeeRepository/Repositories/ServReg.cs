using EmployeeApplication.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRepository.Repositories
{
    public static class ServReg
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IEmployeeRepository, EmployeeRep>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
