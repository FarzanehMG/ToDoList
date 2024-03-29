﻿using Microsoft.EntityFrameworkCore;
using Shared;
using SimpleToDoList.Application.Contracts.Project;
using SimpleToDoList.Application.Contracts.ProjectTask;
using SimpleToDoList.Domain;
using SimpleToDoList.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleToDoList.Application
{
    public class ProjectApplication : IProjectApplication
    {
        private readonly ToDoContext _todoContext;

        public ProjectApplication(ToDoContext todoContext)
        {
            _todoContext = todoContext;
        }

       

        public string Create(CreateProject command)
        {
            var employee = _todoContext.Employees.Find(command.EmployeeId);

            if (employee == null)
            {
                return "Employee does not exist.";
            }

            var newProject = new Project(command.Name);
            _todoContext.Projects.Add(newProject);
            //var res =  _todoContext.Projects.Add(newProject).Entity;

            //if (employee.Projects == null)
            //{
            //    employee.Projects = new List<Project>(); // Initialize the Projects collection if necessary
            //}
            ////newProject.EmployeeId = employee;
            //employee.Projects.Add(res);
            _todoContext.SaveChanges();
            return "Operation is Succeeded";
        }

        public List<ProjectViewModel> GetAllProject()
        {

            /*return _todoContext.Projects
                .Include(project => project.Employees)
                .SelectMany(project => project.Employees, (project, employee) => new ProjectViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    IsActive = project.IsActive,
                    CreationDate = project.CreationDate.ToString(),
                    EmployeeId = employee.Id
                })
                .ToList();*/
            
            var projects = _todoContext.Projects
                .Include(project => project.Employees)
                .Select(project => new ProjectViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    IsActive = project.IsActive,
                    CreationDate = project.CreationDate.ToString(),
                    EmployeeId = project.Employees.Select(employee => employee.Id).ToList(),
                    AccountId = project.Employees.FirstOrDefault() != null ? project.Employees.First().AccountId : Guid.Empty
                    //EmployeeId = project.Employees.Any() ? project.Employees.FirstOrDefault().Id : Guid.Empty

                })
                .ToList();

            return projects;

        }

        public ProjectViewModel GetProjectById(Guid id)
        {
            var project = _todoContext.Projects
                .Include(p => p.Employees)
                .FirstOrDefault(p => p.Id == id);

            if (project == null)
            {
                return null;
            }

            var viewModel = new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                IsActive = project.IsActive,
                CreationDate = project.CreationDate.ToString(),
            };

            if (project.Employees != null && project.Employees.Any())
            {
                viewModel.EmployeeId = project.Employees.Select(employee => employee.Id).ToList();
                viewModel.AccountId = project.Employees.First().AccountId;
            }
            else
            {
                viewModel.EmployeeId = new List<Guid>(); // or set to null if it's nullable
                viewModel.AccountId = Guid.Empty; // or set to null if it's nullable
            }

            return viewModel;
            /*var project = _todoContext.Projects.Find(id);

            if (project == null)
            {
                return null;
            }

            return new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                IsActive = project.IsActive,
                CreationDate = project.CreationDate.ToString(),
                EmployeeId = project.Employees.Select(employee => employee.Id).ToList(),
                AccountId = project.Employees.FirstOrDefault() != null ? project.Employees.First().AccountId : Guid.Empty
            };*/

        }

        public List<ProjectViewModel> GetEmployeeProjects(Guid accountId)
        {
            /*var employee = _todoContext.Employees.Find(employeetId);
            var employeeProject = _todoContext.Employees.Where(x => x.Id == employeetId);
            var accountId = _todoContext.Employees.Select(x => x.AccountId);
            var project = _todoContext.Projects.Select(project => new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                IsActive = project.IsActive,
                AccountId = accountId
            });
            */

            var employee = _todoContext.Employees
                .Include(e => e.Projects) // Include projects navigation property
                .FirstOrDefault(e => e.AccountId == accountId);

            if (employee != null)
            {
                // Now, employee.Projects contains the projects related to the employee
                var projectViewModels = employee.Projects.Select(project => new ProjectViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    IsActive = project.IsActive,
                    CreationDate = Tools.ToPersianDate(project.CreationDate),
                    //EmployeeId = project.Employees.FirstOrDefault() != null ? project.Employees.First().Id : Guid.Empty,
                    AccountId = project.Employees.FirstOrDefault() != null ? project.Employees.First().AccountId : Guid.Empty
                }).ToList();

                return projectViewModels;
            }

            return new List<ProjectViewModel>();

            /*var employee = _todoContext.Employees.Find(employeetId);

            return _todoContext.Projects
                .Where(t => t.employeetId == employeetId)
                .Select(project => new ProjectViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    IsActive = project.IsActive,
                    CreationDate = project.CreationDate.ToString(),
                    AccountId = project.AccountId
                })
                .ToList();*/

            /*var employee = _todoContext.Employees.Find(employeetId);

            if (employee == null)
            {
                // Handle the case where the employee is not found
                // You might want to return an empty list or throw an exception
                return new List<ProjectViewModel>();
            }

            return _todoContext.Projects
                .Where(employee => employee.Id == employeetId)
                .Select(project => new ProjectViewModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    IsActive = project.IsActive,
                    CreationDate = project.CreationDate.ToString(),
                    AccountId = project.AccountId
                })
                .ToList();
            */
        }

        public string Update(UpdateProject command)
        {
            var existingProject = _todoContext.Projects.Find(command.Id);

            if (existingProject == null)
            {
                return "Record Not Found";
            }
            else if (_todoContext.ToDos.Any(x => x.Name == command.Name))
            {
                return "Duplicated Record";
            }
            else
            {
                existingProject.UpdateProject(command.Name,command.IsActive);
                _todoContext.SaveChanges();
                return "Operation is Succedded";
            }
        }

        public string AssignProject(AssignProject command)
        {
            var project = _todoContext.Projects.Find(command.ProjectId);

            if (project == null)
            {
                return "Project not found.";
            }

            var employee = _todoContext.Employees.Find(command.EmployeeId);

            if (employee == null)
            {
                return "Employee not found.";
            }

            // Ensure that task.Employees is initialized
            if (project.Employees == null)
            {
                project.Employees = new List<Employee>();
            }

            // Update the reference to the employee
            //task.Employees.Clear();  // Assuming that you want to replace all existing employees
            project.Employees.Add(employee);

            _todoContext.SaveChanges();

            return "Project assigned to the employee successfully.";
        }

        public List<AssignProjectFilterViewModel> FilterAssignProjectByIsComplete(AssignProject command)
        {
            var employee = _todoContext.Employees
                .Include(e => e.ProjectTasks)
                .ThenInclude(pt => pt.Projects)
                .FirstOrDefault(e => e.Id == command.EmployeeId);

            if (employee == null)
            {
                // Return an error message or handle the situation accordingly
                return new List<AssignProjectFilterViewModel> { new AssignProjectFilterViewModel { ErrorMessage = "Employee not found." } };
            }

            var incompleteTasks = employee.ProjectTasks.Where(x => !x.IsComplete).ToList();

            if (incompleteTasks.Count != 0)
            {
                var tasks = incompleteTasks.Select(x => new AssignProjectFilterViewModel
                {
                    TaskName = x.Name,
                    IsCompleted = x.IsComplete
                }).ToList();

                return tasks;
            }

            AssignProject(command);
            return new List<AssignProjectFilterViewModel>();


            /*var employee = _todoContext.Employees
                .Include(e => e.ProjectTasks)
                .ThenInclude(pt => pt.Projects)
                .FirstOrDefault(e => e.Id == command.EmployeeId);

            var projectTaskViewModels = employee.ProjectTasks.Where(x => x.IsComplete == false).Select(task => new ProjectTaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                IsComplete = task.IsComplete,
                CreationDate = task.CreationDate.ToString(),
                ProjectName = task.Projects.FirstOrDefault() != null ? task.Projects.First().Name : string.Empty,
                ProjectId = task.Projects.Select(p => p.Id).ToList(),
                EmployeeId = task.Employees.Select(e => e.Id).ToList(),
                AccountId = task.Employees.FirstOrDefault() != null ? task.Employees.First().AccountId : Guid.Empty
            }).ToList();

            if (projectTaskViewModels is null)
            {             
                return "Project cannot assigned to the employee.";
            }

            AssignProject(command);*/
        }

    }
}
