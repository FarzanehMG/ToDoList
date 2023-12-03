using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application.Contracts.ProjectTask
{
    public class CreateProjectTask
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
