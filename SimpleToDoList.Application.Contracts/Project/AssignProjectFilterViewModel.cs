using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application.Contracts.Project
{
    public class AssignProjectFilterViewModel
    {
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }
        public string ErrorMessage { get; set; }

    }
}
