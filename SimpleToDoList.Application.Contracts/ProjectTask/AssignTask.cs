﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application.Contracts.ProjectTask
{
    public class AssignTask
    {
        public Guid AdminId { get; set; }
        public string AdminUsername { get; set; }
        public Guid EmployeeId { get; set; }
        public String Fullname { get; set; }
        public string Username { get; set; }
        public Guid TaskId { get; set; }
    }
}
