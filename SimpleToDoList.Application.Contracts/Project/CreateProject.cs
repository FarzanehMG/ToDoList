

using System;

namespace SimpleToDoList.Application.Contracts.Project
{
    public class CreateProject
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
