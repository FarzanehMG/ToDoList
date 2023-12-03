using System;
using System.Collections.Generic;


namespace SimpleToDoList.Application.Contracts.Project
{
    public interface IProjectApplication
    {
        public string Create(CreateProject command);
        public string Update(UpdateProject command);
        public List<ProjectViewModel> GetAllProject();
        public ProjectViewModel GetProjectById(Guid id);
        public string AssignProject(AssignProject command);

        public List<ProjectViewModel> GetEmployeeProjects(Guid employeeId);

        public List<AssignProjectFilterViewModel> FilterAssignProjectByIsComplete(AssignProject command);
    }
}
