using System;
using System.Collections.Generic;

namespace SimpleToDoList.Domain.ToDo
{
    public class ToDo
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public bool IsComplete { get; private set; }
        public DateTime CreationDate { get; private set; }



        public Account Account { get; set; }
        public Guid AccountId { get; private set; }


        public ProjectTask ProjectTask { get; set; }
        public Guid? TaskId { get; set; }


        public ToDo(string name, Guid accountId,Guid taskId)
        {
            Name = name;
            IsComplete = false;
            CreationDate = DateTime.Now;
            AccountId = accountId;
            TaskId = taskId;
        }

        public ToDo(string name, Guid accountId, Guid? taskId)
        {
            Name = name;
            AccountId = accountId;
            TaskId = taskId;
        }

        public void EditToDo(string name)
        {
            Name = name;
        }
        public void Complete()
        {
            IsComplete = true;
        }
        public void NotComplete()
        {
            IsComplete = false;
        }

    }
}
