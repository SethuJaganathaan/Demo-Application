using Application.Repository.Enums;
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

        [Authorize(Policy = CommonConstant.Policies.UserAndAdminPolicy)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            if(users != null)
                return Ok(users);
            else
                return NotFound();
        }

        [Authorize(Policy = CommonConstant.Policies.UserAndAdminPolicy)]
        [HttpDelete("softdelete")]
        public async Task<IActionResult> SoftDeleteUser(Guid userId)
        {
            var success = await _userService.SoftDeleteUser(userId);

            if (success == true)
                return Ok(true);
            else if (success == false)
                return Ok(false);
            else
                return NotFound();
        }
    }
}
