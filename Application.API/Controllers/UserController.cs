using Application.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = "UserOrAdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [Authorize(Policy = "UserOrAdminPolicy")]
        [HttpDelete]
        public async Task<IActionResult> SoftDeleteUser(Guid userId)
        {
            var success = await _userService.SoftDeleteUser(userId);
            return Ok(success);
        }
    }
}
