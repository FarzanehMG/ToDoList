using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleToDoList.Application.Contracts;
using System;

namespace SimpleToDoList.Pages
{
    public class EditModel : PageModel
    {
        public Guid Id { get; set; }
        public string EditToDoName { get; set; }
        public EditToDo EditToDo { get; set; }

        private readonly IToDoApplication _toDoApplication;

        public EditModel(IToDoApplication toDoApplication)
        {
            _toDoApplication = toDoApplication;
        }

        public void OnGet(Guid id)
        {
            Id = id;
            var todo = _toDoApplication.GetTodoById(id);
            EditToDoName = todo.Name;
        }


        public IActionResult OnPostEditToDo(Guid id,string editToDoName)
        {
            _toDoApplication.Edit(new EditToDo { Id = id, Name = editToDoName });
            return RedirectToPage("/ToDoList");
        }
    }
}
