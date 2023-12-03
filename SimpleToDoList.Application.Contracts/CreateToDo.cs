using System;

namespace SimpleToDoList.Application.Contracts
{
    public class CreateToDo
    {
        public string Name { get; set; }
        public Guid AccountId { get; set; }
        public Guid? TaskId { get; set; }
    }
}
