using System;


namespace SimpleToDoList.Application.Contracts.Project
{
    public class UpdateProject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
    }
}
