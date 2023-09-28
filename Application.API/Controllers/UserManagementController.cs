using Application.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> UserManagementByRoleid(Guid Roleid)
        {
            var roles = await _userManagementService.UserManagementByRoleid(Roleid);
            return Ok(roles);
        }
    }
}
