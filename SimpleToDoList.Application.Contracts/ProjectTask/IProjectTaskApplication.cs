using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application.Contracts.ProjectTask
{
    public interface IProjectTaskApplication
    {
        public string Create(CreateProjectTask command);
        public string Edit(EditProjectTask command);
        public List<ProjectTaskViewModel> GetAllTasks();
        public ProjectTaskViewModel GetTaskById(Guid id);
        public string AssignTask(AssignTask command); 

        //public List<ProjectTaskViewModel> GetEmployeeTasks(Guid employeeId);

    }
}
