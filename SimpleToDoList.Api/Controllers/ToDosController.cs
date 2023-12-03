using Microsoft.AspNetCore.Mvc;
using SimpleToDoList.Application.Contracts;
using System;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;


namespace SimpleToDoList.API.Controllers
{
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoApplication _toDoApplication;

        public ToDosController(IToDoApplication toDoApplication)
        {
            _toDoApplication = toDoApplication;
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateToDo command)
        {
            /*if (command == null)
                return BadRequest(ModelState);
            var result = _toDoApplication.Create(command);

            return Ok(result);*/



            /*var loggedInAccountId = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Set the AccountId property of the command
            command.AccountId = loggedInAccountId;

            if (command == null)
                return BadRequest(ModelState);
            var result = _toDoApplication.Create(command);

            return Ok(result);*/




            /* var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

             if (accountIdClaim == null || !Guid.TryParse(accountIdClaim.Value, out var loggedInAccountId))
             {
                 // Log the claims for debugging
                 var userClaims = User.Claims.Select(c => $"{c.Type}: {c.Value}");
                 Console.WriteLine($"User Claims: {string.Join(", ", userClaims)}");

                 // Handle the case where the user is not authenticated or the claim is not available
                 return BadRequest("User not authenticated.");
             }

             // Set the AccountId property of the command
             command.AccountId = loggedInAccountId;

             var result = _toDoApplication.Create(command);

             return Ok(result);*/


            /*Guid loggedInAccountId = GetLoggedInAccountId();

             if (loggedInAccountId == Guid.Empty)
             {
                 return BadRequest(ModelState);
             }

             var result = _toDoApplication.Create(command);

             return Ok(result);*/

            // Retrieve token from headers
            /*string token = HttpContext.Request.Headers["Authorization"];

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


                    // Access claims
                    //string accountIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
                    string accountIdClaim = nameIdValue;

                    if (accountIdClaim != null)
                    {
                        if (Guid.TryParse(accountIdClaim, out var accountId))
                        {
                            command.AccountId = accountId;
                            // Continue with your existing logic
                            var result = _toDoApplication.Create(command);
                            return Ok(result);
                        }
                        else
                        {
                            // Invalid format for the account ID in the token
                            return Unauthorized(new { Message = "Unauthorized: Invalid format for the account ID in the token" });
                        }
                    }
                    else
                    {
                        // "sub" claim not found in the token
                        return Unauthorized(new { Message = "Unauthorized: claim not found in the token" });
                    }
                }
                catch (Exception ex)
                {
                    // Handle token decoding errors
                    Console.WriteLine($"Token decoding error: {ex.Message}");
                    return Unauthorized(new { Message = "Unauthorized: Invalid token format" });
                }
            }
            else
            {
                // No or invalid token provided, return an error response
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }*/


            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            // Set the AccountId property of the command
            command.AccountId = loggedInAccountId;


            // Convert CreateToDo to LimiteToCreateToDo for the limit check
            var limiteCommand = new LimiteToCreateToDo
            {
                AccountId = command.AccountId,
                CreationDate = DateTime.Today
            };

            // Check for the daily limit before creating a new to-do
            bool limitResult = _toDoApplication.LimiteAddToDoInOneDay(limiteCommand);

            if (!limitResult)
            {
                // Return the limit result as a response
                return BadRequest(new { Message = "Client has reached the maximum allowed to-do items for the day." });
            }


            // Continue with your existing logic
            var result = _toDoApplication.Create(command);
            return Ok(result);
        }

        /*private Guid GetLoggedInAccountId()
        {
            if (HttpContext.Session.TryGetValue("AccountId", out var accountIdBytes) && accountIdBytes.Length == 16)
            {
                // Successfully retrieved account ID from session
                if (Guid.TryParse(Encoding.UTF8.GetString(accountIdBytes), out var accountId))
                {
                    return accountId;
                }
            }

            // User not logged in or session doesn't contain a valid account ID
            return Guid.Empty;
        }*/


        /*private Guid GetLoggedInAccountId()
        {
            if (HttpContext.Session.TryGetValue("AccountId", out var accountIdBytes) && accountIdBytes.Length == 16)
            {
                // Successfully retrieved account ID from session
                return new Guid(accountIdBytes);
            }

            // User not logged in or session doesn't contain the account ID
            return Guid.Empty;
        }*/


        [HttpPut("Edit")]
        public IActionResult Edit(EditToDo command)
        {
            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            // Check if the logged-in user has the right to edit the todo
            var todo = _toDoApplication.GetTodoById(command.Id);

            todo.AccountId = loggedInAccountId;

            if (todo == null || todo.AccountId != loggedInAccountId)
            {
                // User does not have the right to edit this todo
                return Unauthorized(new { Message = "Unauthorized: User does not have the right to edit this todo" });
            }

            // Continue with your existing logic
            var result = _toDoApplication.Edit(command);
            return Ok(result);
        }

        [HttpGet("GetUserToDos/{accountId}")]
        public IActionResult GetUserToDos(Guid accountId)
        {
            Guid loggedInAccountId = GetLoggedInAccountId();
            //accountId = loggedInAccountId;

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            // Check if the logged-in user has the right to access the todos of the specified account
            if (loggedInAccountId != accountId)
            {
                // User does not have the right to access the todos of the specified account
                return Unauthorized(new { Message = "Unauthorized: User does not have the right to access the todos of the specified account" });
            }

            // Continue with your existing logic
            var result = _toDoApplication.GetUserToDos(accountId);
            return Ok(result);
        }

        [HttpGet("GetUserToDosParams/{accountId}")]
        public IActionResult GetUserToDosParams(Guid accountId,[FromQuery] ToDoParam toDoParam)
        {
            Guid loggedInAccountId = GetLoggedInAccountId();
            //accountId = loggedInAccountId;

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            // Check if the logged-in user has the right to access the todos of the specified account
            if (loggedInAccountId != accountId)
            {
                // User does not have the right to access the todos of the specified account
                return Unauthorized(new { Message = "Unauthorized: User does not have the right to access the todos of the specified account" });
            }

            var todos = _toDoApplication.GetUserToDosParams(accountId,toDoParam);
            return Ok(todos);
        }


        /*[HttpPut("Edit")]
        public IActionResult Edit(EditToDo command)
        {
            if (command == null)
                return NotFound();

            var result = _toDoApplication.Edit(command);

            return Ok(result);
            
        }

        [HttpGet("GetUserToDos/{accountId}")]
        public IActionResult GetUserToDos(Guid accountId)
        {
            var result = _toDoApplication.GetUserToDos(accountId);
            return Ok(result);
        }*/

        [HttpGet("GetAllToDos")]
        public IActionResult GetAllToDos()
        {
            var todos = _toDoApplication.GetAllToDos();

            return Ok(todos);
        }

        [HttpGet("GetTodoById/{id}")]
        public IActionResult GetTodoById(Guid id)
        {
            var todo = _toDoApplication.GetTodoById(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpPut("Completed/{id}")]
        public IActionResult Completed(Guid id)
        {
            var result = _toDoApplication.Completed(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Operation is Succeeded");
        }

        [HttpPut("NotCompleted/{id}")]
        public IActionResult NotCompleted(Guid id)
        {
            var result = _toDoApplication.NotCompleted(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok("Operation is Succeeded");
        }


        /*[HttpGet("Filter/{accountId}/{isComplete}/{creationDate}")]
        public IActionResult Filter(Guid accountId, bool isComplete, DateTime creationDate)
        {
            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            // Check if the logged-in user has the right to access the todos of the specified account
            if (loggedInAccountId != accountId)
            {
                // User does not have the right to access the todos of the specified account
                return Unauthorized(new { Message = "Unauthorized: User does not have the right to access the todos of the specified account" });
            }

            var command = new FilterStatus
            {
                AccountId = accountId,
                IsComplete = isComplete,
                CreationDate = creationDate
                
            };

            var todos = _toDoApplication.Filter(command);

            return Ok(todos);
        }*/


        [HttpGet("Filter/{accountId}")]
        public IActionResult Filter(Guid accountId, [FromQuery] FilterStatus command)
        {
            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            // Check if the logged-in user has the right to access the todos of the specified account
            if (loggedInAccountId != accountId)
            {
                // User does not have the right to access the todos of the specified account
                return Unauthorized(new { Message = "Unauthorized: User does not have the right to access the todos of the specified account" });
            }


            var todos = _toDoApplication.Filter(accountId,command);

            return Ok(todos);
        }


        [HttpGet("Search/{accountId}")]
        public IActionResult Search(Guid accountId,SearchToDo search)
        {
            Guid loggedInAccountId = GetLoggedInAccountId();

            if (loggedInAccountId == Guid.Empty)
            {
                return Unauthorized(new { Message = "Unauthorized: No or invalid token provided" });
            }

            // Check if the logged-in user has the right to access the todos of the specified account
            if (loggedInAccountId != accountId)
            {
                // User does not have the right to access the todos of the specified account
                return Unauthorized(new { Message = "Unauthorized: User does not have the right to access the todos of the specified account" });
            }

            var todos = _toDoApplication.Search(search);

            return Ok(todos);
        }





        private Guid GetLoggedInAccountId()
        {
            /*// Extract the token from the headers
            string token = HttpContext.Request.Headers["Authorization"];

            // Validate and process the token
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                // Extract the actual token value
                string actualToken = token.Substring("Bearer ".Length).Trim();

                try
                {
                    // Your token decoding logic goes here
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(actualToken) as JwtSecurityToken;

                    // Access claims
                    string accountIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

                    if (accountIdClaim != null)
                    {
                        if (Guid.TryParse(accountIdClaim, out var accountId))
                        {
                            return accountId;
                        }
                        else
                        {
                            // Invalid format for the account ID in the token
                            Console.WriteLine("Invalid format for the account ID in the token");
                        }
                    }
                    else
                    {
                        // "sub" claim not found in the token
                        Console.WriteLine("'sub' claim not found in the token");
                    }
                }
                catch (Exception ex)
                {
                    // Handle token decoding errors
                    Console.WriteLine($"Token decoding error: {ex.Message}");
                }
            }
            else
            {
                // No or invalid token provided
                Console.WriteLine("No or invalid token provided");
            }

            // User not authenticated or token processing failed
            return Guid.Empty;*/
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


                    // Access claims
                    //string accountIdClaim = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
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