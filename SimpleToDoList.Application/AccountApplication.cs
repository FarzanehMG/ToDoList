using Microsoft.AspNetCore.Http;
using Shared;
using SimpleToDoList.Application.Contracts.Account;
using SimpleToDoList.Domain;
using SimpleToDoList.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleToDoList.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly ToDoContext _toDoContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountApplication(ToDoContext toDoContext, IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _toDoContext = toDoContext;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }


        public string Create(CreateAccount command)
        {
            if (_toDoContext.Accounts.Any(x => x.Username == command.Username))
            {
                return "This Username Exists.";
            }

            if (_toDoContext.Accounts.Any(x => x.NationalCode == command.NationalCode))
            {
                return "This NationalCode Exists.";
            }

            var pass = _passwordHasher.Hash(command.Password);

            var account = new Account(command.Fullname, command.Username, pass, command.Mobile,command.NationalCode);
            _toDoContext.Add(account);
            _toDoContext.SaveChanges();
            return "Operation is Succedded";
        }


        public string Edit(UpdateAccount command)
        {
            var account = _toDoContext.Accounts.Find(command.Id);

            if (account == null)
            {
                return "Record Not Found";
            }
            else
            {
                // Check if the new password is provided and not null
                if (command.NewPassword != null)
                {
                    // Proceed with the update, hashing the new password
                    var newHashedPassword = _passwordHasher.Hash(command.NewPassword);

                    // Update the account information
                    account.Update(command.Username, newHashedPassword, command.Fullname, command.Mobile,command.NationalCode);
                }
                else
                {
                    // Update the account information without changing the password
                    account.Update(command.Username, account.Password, command.Fullname, command.Mobile,command.NationalCode);
                }

                _toDoContext.SaveChanges();

                return "Operation is Successful";
            }
        }

        public ViewModelAccount GetAccountBy(Guid id)
        {
            var account = _toDoContext.Accounts.Find(id);

            if (account == null)
            {
                return null;
            }

            return new ViewModelAccount
            {
                Id = account.Id,
                Fullname = account.Fullname,
                Mobile = account.Mobile,
                Password = account.Password,
                Username = account.Username,
                NationalCode = account.NationalCode,
                CreationDate = account.CreationDate.ToString()
            };
        }

        public List<ViewModelAccount> GetAccounts()
        {
            return _toDoContext.Accounts.Select(x => new ViewModelAccount
            {
                Id = x.Id,
                Fullname = x.Fullname,
                Username = x.Username,
                Password = x.Password,
                Mobile = x.Mobile,
                NationalCode = x.NationalCode,
                CreationDate = x.CreationDate.ToString()
            }).ToList();
        }

        public Guid Login(Login command)
        {
            var account = _toDoContext.Accounts.FirstOrDefault(x => x.Username == command.Username);

            if (account == null)
            {
                // User not found
                return Guid.Empty; // or any other indicator that login failed
            }
            

            var resultCheck = _passwordHasher.Check(account.Password, command.Password);

            if (!resultCheck.Verified)
            {
                // Password incorrect
                return Guid.Empty; // or any other indicator that login failed
            }

            // Login successful, return the account ID
            return account.Id;
        }

        public void Logout()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                // Remove the "AccountId" key from the session to signify that the user is logged out
                _httpContextAccessor.HttpContext.Session.Remove("AccountId");
            }
        }
    }
}
