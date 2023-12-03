using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleToDoList.Application;
using SimpleToDoList.Application.Contracts;
using System;
using System.Collections.Generic;
using SimpleToDoList.Application.Contracts.Account;
using Microsoft.AspNetCore.Authentication;

namespace SimpleToDoList.Pages
{
    
    public class IndexModel : PageModel
    {
        public Guid AccountId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public int NationalCode { get; set; }

        private readonly IAccountApplication _accountApplication;

        public IndexModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        public void OnGet(Guid id)
        {
            
        }

        public IActionResult OnPostRegister(string fullName, string mobile, string password, string username,int nationalCode)
        {
            _accountApplication.Create(new CreateAccount
                { Fullname = fullName, Mobile = mobile, Password = password, Username = username, NationalCode = nationalCode });
            return Page();
        }


        public IActionResult OnPostLogin(string username, string password)
        {
                var accountId = _accountApplication.Login(new Login { Username = username, Password = password});

            if (accountId != Guid.Empty)
            {
                // Successfully logged in, store the account ID in the session
                HttpContext.Session.Set("AccountId", accountId.ToByteArray());

                // Redirect to ToDoList page with the retrieved account ID
                return RedirectToPage("EmployeesAssignment", new { accountId });
            }
            else
            {
                // Login failed, handle accordingly (show an error message, redirect to login page, etc.)
                return RedirectToPage("Index");
            }
        }
        /* public IActionResult OnPostLogin(string username, string password)
         {
             var accountId = _accountApplication.Login(new Login { Username = username, Password = password });

             if (accountId != Guid.Empty)
             {
                 return RedirectToPage("ToDoList", new { accountId });
             }
             else
             {
                 return RedirectToPage("LoginError");
             }
         }*/
    }
}


