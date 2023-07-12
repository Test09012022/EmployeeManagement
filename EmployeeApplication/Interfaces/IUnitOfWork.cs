using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeApplication.Interfaces
{
    
        public interface IUnitOfWork
        {
            public IEmployeeRepository Employees { get; }
        }
       
    
}
