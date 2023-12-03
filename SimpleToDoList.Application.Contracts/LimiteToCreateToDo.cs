using System;


namespace SimpleToDoList.Application.Contracts
{
    public class LimiteToCreateToDo
    {
        public Guid AccountId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
