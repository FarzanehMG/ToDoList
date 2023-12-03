using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleToDoList.Application.Contracts.Account;
using System;

namespace SimpleToDoList.Pages
{
    public class EditAccountModel : PageModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Mobile { get; set; }
        public string NewPassword { get; set; }
        public UpdateAccount updateAccount { get; set; }

        private readonly IAccountApplication _accountApplication;

        public EditAccountModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        public void OnGet()
        {
            Guid loggedInAccountId = GetLoggedInAccountId();
            Id = loggedInAccountId;
            var account = _accountApplication.GetAccountBy(loggedInAccountId);
            FullName = account.Fullname;
            Username = account.Username;
            Mobile = account.Mobile;
            //Password = account.Password;
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

        public IActionResult OnPostUpdateAccount(Guid id, string Fullname, string Username, string Mobile, string newPassword)
        {
            _accountApplication.Edit(new UpdateAccount { Id = id, Fullname = Fullname, Username = Username, Mobile = Mobile,NewPassword= newPassword });
            return RedirectToPage("ToDoList");
        }

    }
}

