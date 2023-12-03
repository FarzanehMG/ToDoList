using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application.Contracts.ProjectTask
{
    public class ProjectTaskViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> EmployeeId { get; set; }
        public Guid AccountId { get; set; }
        public List<Guid> ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool IsComplete { get; set; }
        public string CreationDate { get; set; }
    }
}
