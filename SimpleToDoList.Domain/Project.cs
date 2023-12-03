using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Domain
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreationDate { get; private set; }



        public List<Employee> Employees { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; }


        public Project(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }

        public void UpdateProject(string name,bool isActive)
        {
            Name = name;
            IsActive = isActive;
        }
    }
}
