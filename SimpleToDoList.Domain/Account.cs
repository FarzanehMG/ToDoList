using System;
using System.Collections.Generic;
using SimpleToDoList.Domain.ToDo;

namespace SimpleToDoList.Domain
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string Fullname { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Mobile { get; private set; }
        public int NationalCode { get; private set; }
        public DateTime CreationDate { get; private set; }




        public List<ToDo.ToDo> ToDos { get; set; }





        public Account(string fullname, string username, string password, string mobile, int nationalCode)
        {
            Id = Guid.NewGuid();
            Fullname = fullname;
            Username = username;
            Password = password;
            Mobile = mobile;
            CreationDate = DateTime.Now;
            NationalCode = nationalCode;
        }
        public void Update(string fullname,string password, string username, string mobile, int nationalCode)
        {
            Fullname = fullname;
            Username = username;
            Mobile = mobile;
            Password = password;
            NationalCode = nationalCode;
        }
    }
}
