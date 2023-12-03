using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleToDoList.Application.Contracts;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SimpleToDoList.Application.Contracts.Account;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace SimpleToDoList.Pages
{
    public class ToDoListModel : PageModel
    {
        public Guid AccountId { get; set; }
        public string NewToDoName { get; set; }
        public bool Check { get; set; }
        public IEnumerable<ViewModelToDo> ToDos { get; set; }

        //public List<ViewModelToDo> Filter { get; set; }
        public SelectList Filter { get; set; }
        public string SelectedStatus { get; set; }

        private readonly IToDoApplication _toDoApplication;
        private readonly IAccountApplication _accountApplication;

        public ToDoListModel(IToDoApplication toDoApplication, IAccountApplication accountApplication)
        {
            _toDoApplication = toDoApplication;
            _accountApplication = accountApplication;
        }


        public IActionResult OnGet()
        {

            //Guid loggedInAccountId = GetLoggedInAccountId();
            //ToDos = _toDoApplication.GetUserToDos(loggedInAccountId);
            //return Page();


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
            ToDos = _toDoApplication.GetUserToDos(loggedInAccountId);
            return Page();

        }



        public IActionResult OnPostCreateToDo(string NewToDoName)
        {
            /*_toDoApplication.Create(new CreateToDo { Name = NewToDoName });
            return RedirectToPage();*/

            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return RedirectToPage("Index");
            }

            _toDoApplication.Create(new CreateToDo { Name = NewToDoName, AccountId = loggedInAccountId });

            return RedirectToPage();
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



        public IActionResult OnGetUpdateCompletionStatus(Guid id, bool isChecked)
        {
            if (isChecked)
            {
                _toDoApplication.Completed(id);
            }
            else
            {
                _toDoApplication.NotCompleted(id);
            }
            return RedirectToPage("/ToDoList");
        }

        /*public IActionResult OnGetFilterCompletionStatus(bool Check)
        {
            Filter = _toDoApplication.Filter(new FilterStatus { IsComplete = Check });
            return RedirectToPage();
        }
        */

        /*public IActionResult OnGetFilterCompletionStatus(bool Check)
        {
            var filteredToDos = _toDoApplication.Filter(new FilterStatus { IsComplete = Check });

            // Update the Filter property to be a SelectList
            Filter = new SelectList(filteredToDos, "Id", "Name");

            // Set the selected status for the dropdown
            SelectedStatus = Check.ToString();

            // Reload the page
            return RedirectToPage();
        }*/

        /*public IActionResult OnPostFilterCompletionStatus(bool Check)
        {
            Guid loggedInAccountId = GetLoggedInAccountId();
            var filteredToDos = _toDoApplication.Filter(new FilterStatus {AccountId=loggedInAccountId, IsComplete = Check });

            ToDos = filteredToDos;

            return Page();
        }*/
        public IActionResult OnPostLogout()
        {
            _accountApplication.Logout();
            return RedirectToPage("Index");
        }

    }
}
