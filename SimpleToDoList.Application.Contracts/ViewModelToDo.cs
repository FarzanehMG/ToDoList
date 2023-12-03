namespace SimpleToDoList.Application.Contracts
{
    public class ViewModelToDo : EditToDo
    {
        public bool IsComplete { get; set; }
        public string CreationDate { get; set; }
    }
}
