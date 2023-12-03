using System;
using System.Collections.Generic;
using System.Linq;
using SimpleToDoList.Application.Contracts;
using SimpleToDoList.Domain;
using SimpleToDoList.Domain.ToDo;
using SimpleToDoList.Infrastructure;

namespace SimpleToDoList.Application
{
    public class ToDoApplication : IToDoApplication
    {
        private readonly ToDoContext _todoContext;

        public ToDoApplication(ToDoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public string Create(CreateToDo command)
        {
            var account = _todoContext.Accounts.Find(command.AccountId);

            if (account == null)
            {
                return "Account does not exist.";
            }

            var newToDo = new ToDo(command.Name, command.AccountId,command.TaskId);
            newToDo.Account = account;

            _todoContext.ToDos.Add(newToDo);
            _todoContext.SaveChanges();
            return "Operation is Succeeded";

        }

        public string Edit(EditToDo command)
        {
            var existingToDo = _todoContext.ToDos.Find(command.Id);

            if (existingToDo == null)
            {
                return "Record Not Found";
            }
            else if (_todoContext.ToDos.Any(x => x.Name == command.Name))
            {
                return "Duplicated Record";
            }
            else
            {
                existingToDo.EditToDo(command.Name);
                _todoContext.SaveChanges();
                return "Operation is Succedded";
            }
        }

        public IEnumerable<ViewModelToDo> GetAllToDos()
        {
            return _todoContext.ToDos.Select(todo => new ViewModelToDo
            {
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete,
                CreationDate = todo.CreationDate.ToString()
            }).ToList();
        }

        public ViewModelToDo GetTodoById(Guid id)
        {
            var todo = _todoContext.ToDos.Find(id);

            if (todo == null)
            {
                return null;
            }

            return new ViewModelToDo
            {
                Id = todo.Id,
                Name = todo.Name,
                IsComplete = todo.IsComplete,
                CreationDate = todo.CreationDate.ToString()
            };
        }

        public bool Completed(Guid id)
        {
            var todo = _todoContext.ToDos.Find(id);

            if (todo == null)
            {
                return false;
            }

            todo.Complete();
            _todoContext.SaveChanges();
            return true;
        }
        public bool NotCompleted(Guid id)
        {
            var todo = _todoContext.ToDos.Find(id);

            if (todo == null)
            {
                return false;
            }

            todo.NotComplete();
            _todoContext.SaveChanges();
            return false;
        }

        /*public List<ViewModelToDo> Filter(FilterStatus command)
        {
            return _todoContext.ToDos
                    .Where(x => x.IsComplete == command.IsComplete)
                    .Where(x=>x.CreationDate.Day == command.CreationDate.Day)
                    .Where(x=>x.AccountId == command.AccountId)
                    .Select(todo => new ViewModelToDo
                    {
                        Id = todo.Id,
                        Name = todo.Name,
                        IsComplete = todo.IsComplete,
                        CreationDate = todo.CreationDate.ToString(),
                        AccountId = todo.AccountId
                    })
                    .ToList();
        }*/


        public List<ViewModelToDo> Filter(Guid accountId, FilterStatus command)
        {
            return _todoContext.ToDos
                    .Where(x => x.IsComplete == command.IsComplete)
                    .Where(x => x.CreationDate.Day == command.CreationDate.Day)
                    .Where(x => x.AccountId == accountId)
                    .Select(todo => new ViewModelToDo
                    {
                        Id = todo.Id,
                        Name = todo.Name,
                        IsComplete = todo.IsComplete,
                        CreationDate = todo.CreationDate.ToString(),
                        AccountId = todo.AccountId
                    })
                    .ToList();
        }

        public List<ViewModelToDo> GetUserToDos(Guid accountId)
        {
            return _todoContext.ToDos
                .Where(t => t.AccountId == accountId)
                .Select(todo => new ViewModelToDo
                {
                    Id = todo.Id,
                    Name = todo.Name,
                    IsComplete = todo.IsComplete,
                    CreationDate = todo.CreationDate.ToString(),
                    AccountId = todo.AccountId
                })
                .ToList();
        }


        public List<ViewModelToDo> GetUserToDosParams(Guid accountId, ToDoParam toDoParam)
        {
            return _todoContext.ToDos
               .Where(t => t.AccountId == accountId)
               .Select(todo => new ViewModelToDo
               {
                   Id = todo.Id,
                   Name = todo.Name,
                   IsComplete = todo.IsComplete,
                   CreationDate = todo.CreationDate.ToString(),
                   AccountId = todo.AccountId
               })
               .OrderBy(on => on.CreationDate)
                .Skip((toDoParam.PageNumber - 1) * toDoParam.PageSize)
                .Take(toDoParam.PageSize)
               .ToList();
        }

        public List<ViewModelToDo> Search(SearchToDo search)
        {
            return _todoContext.ToDos
                .Select(todo => new ViewModelToDo
                {
                    Id = todo.Id,
                    Name = todo.Name,
                    IsComplete = todo.IsComplete,
                    CreationDate = todo.CreationDate.ToString(),
                    AccountId = todo.AccountId
                }).Where(x => x.Name == search.Name).ToList();
        }

        public bool LimiteAddToDoInOneDay(LimiteToCreateToDo command)
        {
            var clientToDoCount = _todoContext.ToDos
                .Count(x => x.AccountId == command.AccountId && x.CreationDate.Date == command.CreationDate.Date);

            return clientToDoCount < 5;
        }

        /*public bool LimiteAddToDoInOneDay(LimiteToCreateToDo command)
        {
            var clientToDoCount = _todoContext.ToDos
            .Count(x => x.AccountId == command.AccountId && x.CreationDate.Date == command.CreationDate.Date);
            if (clientToDoCount >= 5)
            {
                return false;
            }

            return true;
        }*/
    }
}


