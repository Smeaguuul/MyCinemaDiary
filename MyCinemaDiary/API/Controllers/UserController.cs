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
                return BadRequest("Username needs to be unique!");
            }

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var username = loginRequest.Username;
            var password = loginRequest.Password;
            var user = await _users.Login(username, password);

            // Generate a JWT token
            var token = GenerateJwtToken(user);

            // Return the token to the client
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(getJWTKey()));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string getJWTKey()
        {
            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

            string filePath = Path.Combine(solutionDirectory, "MyCinemaDiary", "secrets.json");
            StreamReader reader = new(filePath);
            var text = reader.ReadToEnd();

            return JsonDocument.Parse(text).RootElement.GetProperty("Jwt").GetProperty("Key").ToString();
        }
    }

}
