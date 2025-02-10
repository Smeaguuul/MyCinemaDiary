using Microsoft.AspNetCore.Mvc;
using MyCinemaDiary.Application;
using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly Users _users;

        public UsersController(Users users)
        {
            _users = users;
        }

        [HttpGet(Name = "GetUser")]
        public async Task<User> Get()
        {
            var id = HttpContext.Request.Query["id"].ToString();

            int userId = int.Parse(id);
            var user = await _users.GetUser(userId);
            return user;
        }

        [HttpPost(Name = "Add User")]
        public async Task<IActionResult> Post(User user)
        {
            await _users.AddUser(user);
            return Created();
        }

    }
}
