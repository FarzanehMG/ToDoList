using Microsoft.EntityFrameworkCore;
using SimpleToDoList.Application.Contracts.Account;
using SimpleToDoList.Application.Contracts.Project;
using SimpleToDoList.Application.Contracts.ProjectTask;
using SimpleToDoList.Domain;
using SimpleToDoList.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application
{
    public class ProjectTaskApplication : IProjectTaskApplication
    {
        private readonly ToDoContext _todoContext;

        public ProjectTaskApplication(ToDoContext todoContext)
        {
            _todoContext = todoContext;
        }


        public string Create(CreateProjectTask command)
        {
            var employee = _todoContext.Employees.Find(command.EmployeeId);

            if (employee == null)
            {
                return "Employee does not exist.";
            }

            var project = _todoContext.Projects.Find(command.ProjectId);

            if (project == null)
            {
                return "Project does not exist.";
            }

            var newTask = new ProjectTask(command.Name);
            var res = _todoContext.ProjectTasks.Add(newTask).Entity;

            if (project.ProjectTasks == null)
            {
                project.ProjectTasks = new List<ProjectTask>();
            }

            /*if (employee.ProjectTasks == null)
            {
                employee.ProjectTasks = new List<ProjectTask>();
            }
            employee.ProjectTasks.Add(res);*/

            project.ProjectTasks.Add(res);
            

            _todoContext.SaveChanges();
            return "Operation is Succeeded";
        }

        public string Edit(EditProjectTask command)
        {
            var existingTask = _todoContext.ProjectTasks.Find(command.Id);

            if (existingTask == null)
            {
                return "Record Not Found";
            }
            else if (_todoContext.ProjectTasks.Any(x => x.Name == command.Name))
            {
                return "Duplicated Record";
            }
            else
            {
                existingTask.EditTask(command.Name,command.IsCompleted);
                _todoContext.SaveChanges();
                return "Operation is Succedded";
            }
        }

        public List<ProjectTaskViewModel> GetAllTasks()
        {
            var tasks = _todoContext.ProjectTasks
                .Include(emp => emp.Employees)
                .Include(project=>project.Projects)
                .Select(task => new ProjectTaskViewModel
                {
                    Id = task.Id,
                    Name = task.Name,
                    IsComplete = task.IsComplete,
                    CreationDate = task.CreationDate.ToString(),
                    EmployeeId = task.Employees.Select(employee => employee.Id).ToList(),
                    AccountId = task.Employees.FirstOrDefault() != null ? task.Employees.First().AccountId : Guid.Empty,
                    ProjectId = task.Projects.Select(project => project.Id).ToList(),
                    ProjectName = task.Projects.Select(project => project.Name).ToString()

                })
                .ToList();

            return tasks; 
        }


        public ProjectTaskViewModel GetTaskById(Guid id)
        {
            var task = _todoContext.ProjectTasks
                .Include(p => p.Employees)
                .Include(p => p.Projects)
                .FirstOrDefault(p => p.Id == id);

            if (task == null)
            {
                return null;
            }

            var viewModel = new ProjectTaskViewModel()
            {
                Id = task.Id,
                Name = task.Name,
                IsComplete = task.IsComplete,
                CreationDate = task.CreationDate.ToString(),
            };

            if (task.Employees != null && task.Employees.Any())
            {
                viewModel.EmployeeId = task.Employees.Select(employee => employee.Id).ToList();
                viewModel.AccountId = task.Employees.First().AccountId;
            }
            else
            {
                viewModel.EmployeeId = new List<Guid>(); // or set to null if it's nullable
                viewModel.AccountId = Guid.Empty; // or set to null if it's nullable
            }

            if (task.Projects != null && task.Projects.Any())
            {
                viewModel.ProjectId = task.Projects.Select(project => project.Id).ToList();
                viewModel.ProjectName = task.Projects.FirstOrDefault()?.Name ?? "No Project"; // Adjust the default string as needed
            }
            else
            {
                viewModel.ProjectId = new List<Guid>(); // or set to null if it's nullable
                viewModel.ProjectName = "No Project";
            }

            return viewModel;
        }

        public string AssignTask(AssignTask command)
        {
            /*var task = _todoContext.ProjectTasks.Find(command.TaskId);

            if (task == null)
            {
                return "Task not found.";
            }

            var employee = _todoContext.Employees.Find(command.EmployeeId);

            if (employee == null)
            {
                return "Employee not found.";
            }

            // Update the reference to the employee
            task.Employees = employee;

            _todoContext.SaveChanges();

            return "Task assigned to the employee successfully.";*/

            
             var task = _todoContext.ProjectTasks.Find(command.TaskId);

            if (task == null)
            {
                return "Task not found.";
            }

            var employee = _todoContext.Employees.Find(command.EmployeeId);

            if (employee == null)
            {
                return "Employee not found.";
            }

            // Ensure that task.Employees is initialized
            if (task.Employees == null)
            {
                task.Employees = new List<Employee>();
            }

            // Update the reference to the employee
            //task.Employees.Clear();  // Assuming that you want to replace all existing employees
            task.Employees.Add(employee);

            _todoContext.SaveChanges();

            return "Task assigned to the employee successfully.";
             
        }
    }
}
