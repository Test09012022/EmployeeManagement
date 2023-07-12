using EmployeeApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeRepository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        public UnitOfWork(IEmployeeRepository employeeRepository)
        {
            Employees = employeeRepository;
        }
        public IEmployeeRepository Employees { get; }
    
    }
}
