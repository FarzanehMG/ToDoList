using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Domain
{
    public class ProjectTask
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsComplete { get; private set; }
        public DateTime CreationDate { get; private set; }


        public List<Employee> Employees { get; set; }
        public List<Project> Projects { get; set; }

        public List<ToDo.ToDo> ToDoes { get; set; }




        public ProjectTask(string name)
        {
            Id= Guid.NewGuid();
            Name = name;
            //IsComplete = false;
            CreationDate = DateTime.Now;
        }

        public void EditTask(string name,bool isCompleted)
        {
            Name = name;
            IsComplete = isCompleted;
        }
        /*public void Complete()
        {
            IsComplete = true;
        }
        public void NotComplete()
        {
            IsComplete = false;
        }*/
    }
}
