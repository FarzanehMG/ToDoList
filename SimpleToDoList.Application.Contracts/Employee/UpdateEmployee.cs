using System;

namespace SimpleToDoList.Application.Contracts.Employee
{
    public class UpdateEmployee
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Salary { get; set; }
        public string Job { get; set; }
        public string Department { get; set; }
        public int NationalCode { get; set; }
    }
}
