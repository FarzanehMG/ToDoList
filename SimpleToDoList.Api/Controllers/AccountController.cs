using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SimpleToDoList.Application.Contracts.Account;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleToDoList.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountApplication accountApplication, IConfiguration configuration)
        {
            _accountApplication = accountApplication;
            _configuration = configuration;
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateAccount command)
        {

            if (command == null)
                return BadRequest(ModelState);
            var result = _accountApplication.Create(command);

            return Ok(result);
        }

        [HttpPut("Edit")]
        public IActionResult Edit(UpdateAccount command)
        {
            if (command == null)
                return NotFound();

            var result = _accountApplication.Edit(command);

            return Ok(result);
        }

        [HttpGet("GetAccountById/{id}")]
        public IActionResult GetAccountBy(Guid id)
        {
            var todo = _accountApplication.GetAccountBy(id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        [HttpGet("GetAllAccounts")]
        public IActionResult GetAccounts()
        {
            var accounts = _accountApplication.GetAccounts();
            return Ok(accounts);
        }

        /*[HttpPost("Login")]
        public IActionResult Login([FromBody] Login command)
        {
            var accountId = _accountApplication.Login(command);

            if (accountId != Guid.Empty)
            {
                return Ok(accountId);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            _accountApplication.Logout();
            return Ok("Logout successful");
        }*/
        [HttpPost("Login")]
        public IActionResult Login(Login command)
        {
            /*var accountId = _accountApplication.Login(command);

            if (accountId != Guid.Empty)
            {
                return Ok(new { Message = "Login successful", AccountId = accountId });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }*/

            /*var accountId = _accountApplication.Login(command);

            if (accountId != Guid.Empty)
            {
                var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, "username"),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
                var token = GenerateToken(authClaims);
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized();
            }*/


            /*var accountId = _accountApplication.Login(new Login { Username = command.Username, Password = command.Password });

            if (accountId != Guid.Empty)
            {
                var token = GenerateJwtToken(accountId.ToString());
                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }*/

            var accountId = _accountApplication.Login(command);

            if (accountId != Guid.Empty)
            {
                var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "username"),
            new Claim(ClaimTypes.NameIdentifier, accountId.ToString()), 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

                var token = GenerateToken(authClaims);

                // Set AccountId in the session
                HttpContext.Session.Set("AccountId", Encoding.ASCII.GetBytes(accountId.ToString()));

                return Ok(new { Token = token });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("Logout")]
        [Authorize] // Requires authentication
        public IActionResult Logout()
        {
            _accountApplication.Logout();
            return Ok("Logout successful");
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
            var _TokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInHour"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Audience = _configuration["JWTKey:ValidAudience"],
                //Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
                Expires = DateTime.UtcNow.AddMinutes(50),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        /*private string GenerateJwtToken(string accountId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Use a more secure method to generate a key
            var key = new byte[32]; // 256 bits
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(key);
            }

            var signingKey = new SymmetricSecurityKey(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", accountId) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }*/


    }
}


