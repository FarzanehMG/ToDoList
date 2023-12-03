using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application.Contracts.Account
{
    public class Login
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        //public int NationalCode { get; set; }
    }
}
