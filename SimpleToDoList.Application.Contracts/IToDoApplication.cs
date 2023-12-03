
using System;
using System.Collections.Generic;

namespace SimpleToDoList.Application.Contracts
{
    public interface IToDoApplication
    {
        public string Create(CreateToDo command);
        public string Edit(EditToDo command);
        public IEnumerable<ViewModelToDo> GetAllToDos();
        public ViewModelToDo GetTodoById(Guid id);
        public bool Completed(Guid id);
        public bool NotCompleted(Guid id);
        //public List<ViewModelToDo> Filter(FilterStatus command);
        public List<ViewModelToDo> Filter(Guid accountId, FilterStatus command);
        public List<ViewModelToDo> GetUserToDos(Guid accountId);

        public List<ViewModelToDo> Search(SearchToDo search);
        public List<ViewModelToDo> GetUserToDosParams(Guid accountId,ToDoParam toDoParam);

        public bool LimiteAddToDoInOneDay(LimiteToCreateToDo command);
    }
}
