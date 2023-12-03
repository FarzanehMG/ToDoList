using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application.Contracts.ProjectTask
{
    public class EditProjectTask
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public bool IsCompleted { get; set; }  
        public Guid EmployeeId { get; set; }
    }
}
