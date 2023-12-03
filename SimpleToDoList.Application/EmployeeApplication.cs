using SimpleToDoList.Application.Contracts.Employee;
using SimpleToDoList.Domain;
using SimpleToDoList.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application
{
    public class EmployeeApplication : IEmployeeApplication
    {
        private readonly ToDoContext _todoContext;

        public EmployeeApplication(ToDoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public string Create(CreateEmployee command)
        {
            var account = _todoContext.Accounts.FirstOrDefault(a => a.NationalCode == command.NationalCode);

            if (account == null)
            {
                return "Account not found for the provided NationalCode.";
            }

            var newEmployee = new Employee(command.Job,command.Salary,command.Department,account.Id, command.Username,command.NationalCode);

            _todoContext.Employees.Add(newEmployee);
            _todoContext.SaveChanges();

            return "Operation is Succeeded";
        }

        public List<EmployeeViewModel> GetAllEmployee()
        {
            return _todoContext.Employees.Select(x => new EmployeeViewModel
            {
                Id = x.Id,
                Salary = x.Salary,
                Job = x.Job,
                Department = x.Department,
                NationalCode = x.NationalCode,
                AccountId = x.AccountId,
                CreationDate = x.CreationDate.ToString()
            }).ToList();
        }

        public EmployeeViewModel GetEmployeeById(Guid id)
        {
            var employee = _todoContext.Employees.Find(id);

            if (employee == null)
            {
                return null;
            }

            return new EmployeeViewModel
            {
                Id = employee.Id,
                Salary = employee.Salary,
                Job = employee.Job,
                Department = employee.Department,
                NationalCode = employee.NationalCode,
                AccountId = employee.AccountId,
                CreationDate = employee.CreationDate.ToString()
            };
        }

        public string Update(UpdateEmployee command)
        {
            var existingEmployee = _todoContext.Employees.Find(command.Id);

            if (existingEmployee == null)
            {
                return "Record Not Found";
            }
            /*else if (_todoContext.Employees.Any(x => x.Username == command.Username))
            {
                return "Duplicated Record";
            }*/
            else
            {
                existingEmployee.UpdateEmployee(command.Job, command.Salary, command.Department, command.Username,command.NationalCode);
                _todoContext.SaveChanges();
                return "Operation is Succedded";
            }
        }
    }
}
