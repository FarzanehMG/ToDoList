using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleToDoList.Application.Contracts.Project;
using System.IdentityModel.Tokens.Jwt;
using System;
using SimpleToDoList.Application.Contracts.ProjectTask;

namespace SimpleToDoList.Api.Controllers
{
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectTaskController : Controller
    {
        private readonly IProjectTaskApplication _projectTaskApplication;

        public ProjectTaskController(IProjectTaskApplication projectTaskApplication)
        {
            _projectTaskApplication = projectTaskApplication;
        }


        [HttpPost("Create")]
        public IActionResult Create(CreateProjectTask command)
        {

            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            if (command.EmployeeId == new Guid("467479CA-5A4B-4425-4266-08DBF2F62A76") && command.Username == "Admin")
            {
                // Only allow the creation if the conditions are met
                var result = _projectTaskApplication.Create(command);
                return Ok(result);
            }
            else
            {
                // If the conditions are not met, return unauthorized
                return Unauthorized(new { Message = "Unauthorized: Insufficient privileges" });
            }

            //command.AccountId = loggedInAccountId;

            //var result = _projectTaskApplication.Create(command);
            //return Ok(result);
        }



        [HttpPut("Edit")]
        public IActionResult Edit(EditProjectTask command)
        {
            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            if (command.EmployeeId == new Guid("0F6D994A-BBAF-42C2-9AED-8DA876AB1F2A") && command.Username == "Admin")
            {
                var task = _projectTaskApplication.GetTaskById(command.Id);

                task.AccountId = loggedInAccountId;

                if (task == null || task.AccountId != loggedInAccountId)
                {
                    return Unauthorized(new { Message = "Unauthorized: User does not have the right to edit this project" });
                }

                var result = _projectTaskApplication.Edit(command);
                return Ok(result);
            }
            else
            {
                // If the conditions are not met, return unauthorized
                return Unauthorized(new { Message = "Unauthorized: Insufficient privileges" });
            }

            
        }


        [HttpGet("GetAllTask")]
        public IActionResult GetAllProject()
        {
            var projectTasks = _projectTaskApplication.GetAllTasks();

            return Ok(projectTasks);
        }

        [HttpGet("GetTaskById/{id}")]
        public IActionResult GetProjectById(Guid id)
        {
            var projectTasks = _projectTaskApplication.GetTaskById(id);

            if (projectTasks == null)
            {
                return NotFound();
            }

            return Ok(projectTasks);
        }

        [HttpPost("AssignTask")]
        public IActionResult AssignTask(AssignTask command)
        {
            

            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            if (command.AdminId == loggedInAccountId )
            {
                var projectTasks = _projectTaskApplication.AssignTask(command);
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

