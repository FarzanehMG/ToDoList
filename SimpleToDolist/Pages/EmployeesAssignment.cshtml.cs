using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleToDoList.Application.Contracts;
using SimpleToDoList.Application.Contracts.Project;
using SimpleToDoList.Application.Contracts.ProjectTask;

namespace SimpleToDolist.Pages
{
    public class EmployeesAssignmentModel : PageModel
    {
        public IEnumerable<ProjectViewModel> Projects { get; set; }
        public IEnumerable<ProjectTaskViewModel> Tasks { get; set; }
        public IEnumerable<ViewModelToDo> Todos { get; set; }

        private readonly IToDoApplication _todoApplication;
        private readonly IProjectApplication _projectApplication;
        private readonly IProjectTaskApplication _taskApplication;

        public EmployeesAssignmentModel(IToDoApplication todoApplication, IProjectApplication projectApplication, IProjectTaskApplication taskApplication)
        {
            _todoApplication = todoApplication;
            _projectApplication = projectApplication;
            _taskApplication = taskApplication;
        }

        public IActionResult OnGet()
        {
          var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "username"),
                // Add other claims as needed
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);



            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            Guid loggedInAccountId = GetLoggedInAccountId();
            Projects = _projectApplication.GetEmployeeProjects(loggedInAccountId);
            Tasks = _taskApplication.GetEmployeeTasks(loggedInAccountId);
            Todos = _todoApplication.GetUserToDos(loggedInAccountId);
            return Page();

        }

        private Guid GetLoggedInAccountId()
        {
            if (HttpContext.Session.TryGetValue("AccountId", out var accountIdBytes))
            {
                // Successfully retrieved account ID from session
                return new Guid(accountIdBytes);
            }

            // User not logged in or session doesn't contain the account ID
            return Guid.Empty;
        }
    }
}
