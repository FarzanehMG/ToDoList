using System;


namespace SimpleToDoList.Application.Contracts
{
    public class EditToDo : CreateToDo
    {
        public Guid Id { get; set; }
    }
}
