using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDoList.Application.Contracts.Project;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleToDoList.Api.Controllers
{

    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectApplication _projectApplication;

        public ProjectController(IProjectApplication projectApplication)
        {
            _projectApplication = projectApplication;
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateProject command)
        {

            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            if (command.EmployeeId == new Guid("467479CA-5A4B-4425-4266-08DBF2F62A76") && command.Username == "Admin")
            {
                // Only allow the creation if the conditions are met
                var result = _projectApplication.Create(command);
                return Ok(result);
            }
            else
            {
                // If the conditions are not met, return unauthorized
                return Unauthorized(new { Message = "Unauthorized: Insufficient privileges" });
            }
        }



        [HttpPut("Edit")]
        public IActionResult Edit(UpdateProject command)
        {
            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            if (/*command.EmployeeId == new Guid("0F6D994A-BBAF-42C2-9AED-8DA876AB1F2A") &&*/ command.Username == "Admin")
            {
                var project = _projectApplication.GetProjectById(command.Id);

                project.AccountId = loggedInAccountId;

                if (project == null || project.AccountId != loggedInAccountId)
                {
                    return Unauthorized(new { Message = "Unauthorized: User does not have the right to edit this project" });
                }

                var result = _projectApplication.Update(command);
                return Ok(result);
            }
            else
            {
                // If the conditions are not met, return unauthorized
                return Unauthorized(new { Message = "Unauthorized: Insufficient privileges" });
            }

            //var project = _projectApplication.GetProjectById(command.Id);

            //project.AccountId = loggedInAccountId;

            //if (project == null || project.AccountId != loggedInAccountId)
            //{
            //    return Unauthorized(new { Message = "Unauthorized: User does not have the right to edit this project" });
            //}

            //var result = _projectApplication.Update(command);
            //return Ok(result);
        }

        [HttpGet("GetEmployeeProjects/{employeeId}")]
        public IActionResult GetUserProjects(Guid employeeId)
        {
            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            /*if (loggedInAccountId != employeeId)
            {
                return Unauthorized(new { Message = "Unauthorized: User does not have the right to access the todos of the specified account" });
            }*/

            var result = _projectApplication.GetEmployeeProjects(employeeId);
            return Ok(result);
        }

        [HttpGet("GetAllProject")]
        public IActionResult GetAllProject()
        {
            var projects = _projectApplication.GetAllProject();

            return Ok(projects);
        }

        [HttpGet("GetProjectById/{id}")]
        public IActionResult GetProjectById(Guid id)
        {
            var projects = _projectApplication.GetProjectById(id);

            if (projects == null)
            {
                return NotFound();
            }

            return Ok(projects);
        }


        [HttpPost("AssignProject")]
        public IActionResult AssignTask(AssignProject command)
        {


            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            if (command.AdminId == loggedInAccountId)
            {
                var projectTasks = _projectApplication.AssignProject(command);
                return Ok(projectTasks);
            }
            else
            {
                // If the conditions are not met, return unauthorized
                return Unauthorized(new { Message = "Unauthorized: Insufficient privileges" });
            }
        }

        private Guid GetLoggedInAccountId()
        {
            string token = HttpContext.Request.Headers["Authorization"];

            // Validate and process the token
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                // Extract the actual token value
                string actualToken = token.Substring("Bearer ".Length).Trim();

                try
                {
                    string nameIdValue = null;
                    // Your token decoding logic goes here
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(actualToken) as JwtSecurityToken;


                    // Log claims for debugging
                    foreach (var claim in jsonToken?.Claims)
                    {
                        //Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                        if (claim.Type == "nameid")
                        {
                            // Store the value of the "nameid" claim in the variable
                            nameIdValue = claim.Value;
                            break;
                        }
                    }


                    string accountIdClaim = nameIdValue;

                    if (accountIdClaim != null)
                    {
                        if (Guid.TryParse(accountIdClaim, out var accountId))
                        {

                            return accountId;
                        }
                        else
                        {
                            return Guid.Empty;
                        }
                    }
                    else
                    {
                        return Guid.Empty;
                    }
                }
                catch (Exception ex)
                {
                    // Handle token decoding errors
                    Console.WriteLine($"Token decoding error: {ex.Message}");
                    return Guid.Empty;
                }
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}
