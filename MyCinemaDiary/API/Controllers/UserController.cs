using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using MyCinemaDiary.API.Models;
using MyCinemaDiary.Application;
using MyCinemaDiary.Domain.Entities;
using System.Text.Json;
using System.Security.Claims;

namespace MyCinemaDiary.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Users _users;

        public UserController(Users users)
        {
            _users = users;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var name = registerRequest.Name;
            var username = registerRequest.Username;
            var password = registerRequest.Password;
            try
            {
                await _users.Register(name, username, password);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var username = loginRequest.Username;
            var password = loginRequest.Password;

            string token;
            try
            {
                token = await _users.Login(username, password);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

            
            // Return the token to the client
            return Ok(new { Token = token });
        }
    }

}
