using Microsoft.AspNetCore.Mvc;
using SimpleToDoList.Application.Contracts.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleToDoList.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeApplication _employeeApplication;

        public EmployeeController(IEmployeeApplication employeeApplication)
        {
            _employeeApplication = employeeApplication;
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateEmployee command)
        {
            if (command == null)
                return BadRequest(ModelState);
            var result = _employeeApplication.Create(command);

            return Ok(result);
        }
        [HttpPut("Edit")]
        public IActionResult Edit(UpdateEmployee command)
        {
            if (command == null)
                return NotFound();

            var result = _employeeApplication.Update(command);

            return Ok(result);
        }

        [HttpGet("GetEmployeeById/{id}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var todo = _employeeApplication.GetEmployeeById(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpGet("GetAllEmployee")]
        public IActionResult GetAllEmployee()
        {
            var accounts = _employeeApplication.GetAllEmployee();
            return Ok(accounts);
        }
    }
}
