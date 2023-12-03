using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Domain
{
    public class Employee
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Salary { get; private set; }
        public string Job { get; private set; }
        public string Department { get; private set; }
        public DateTime CreationDate { get; private set; }
        public int NationalCode { get; private set; }



        public List<Project> Projects { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; }
        public Guid AccountId { get; private set; }




        public Employee( string salary, string job, string department, Guid accountId,string username,int nationalCode)
        {
            Id = new Guid();
            CreationDate = DateTime.Now;
            Salary = salary;
            Job = job;
            Department = department;
            AccountId = accountId;
            Username = username;
            NationalCode = nationalCode;
        }

        public void UpdateEmployee( string salary, string job, string department, string username, int nationalCode)
        {
            Salary = salary;
            Job = job;
            Department = department;
            Username = username;
            NationalCode = nationalCode;
        }
    }
}
