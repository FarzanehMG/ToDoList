using System;
using System.Collections.Generic;


namespace SimpleToDoList.Application.Contracts.Project
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string CreationDate { get; set; }
        public List<Guid> EmployeeId { get; set; }
        public Guid AccountId { get; set; }
    }
}
