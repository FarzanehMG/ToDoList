using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Application.Contracts
{
    public class FilterStatus
    {
        public Guid Id { get; set; }
        public bool IsComplete { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
