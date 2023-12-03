using System;
using System.Collections.Generic;

namespace SimpleToDoList.Application.Contracts.Employee
{
    public interface IEmployeeApplication
    {
        public string Create(CreateEmployee command);
        public string Update(UpdateEmployee command);
        public List<EmployeeViewModel> GetAllEmployee();
        public EmployeeViewModel GetEmployeeById(Guid id);
    }
}
